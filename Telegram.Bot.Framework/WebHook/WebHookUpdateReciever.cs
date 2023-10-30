using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.WebHook
{
    //Короче надо предусмотреть вывод в лог если недостаточно прав
    public class WebHookUpdateReciever : IUpdateReciever
    {
        private ILogger<WebHookUpdateReciever> _logger;

        private WebHookOptions _options;

        private ITelegramBotClient _botClient;

        public WebHookUpdateReciever(ITelegramBotClient botClient, IOptions<WebHookOptions> options, ILogger<WebHookUpdateReciever> logger)
        {
            _logger = logger;
            _options = options.Value;
            _botClient = botClient;
        }

        public async Task RecieveAsync(IUpdateHandler handler, CancellationToken cancellation)
        {
            _logger.LogDebug("WebHook starting");

            if (_options.WebHookUrl != null)
                await SetWebHook();

            HttpListener httpListener = new HttpListener()
            {
                Prefixes = { _options.ListenUrl }
            };
            httpListener.Start();
            _logger.LogDebug("WebHook started");
            cancellation.Register(() => httpListener.Stop());
            while (!cancellation.IsCancellationRequested)
            {
                try
                {
                    HttpListenerContext context = await httpListener.GetContextAsync();
                    #pragma warning disable CS4014
                    Task.Run(() => HandleContext(handler, context, cancellation));
                    #pragma warning restore CS4014
                }
                catch (OperationCanceledException)
                {

                }
                catch (ObjectDisposedException)
                {

                }
                catch (HttpListenerException)
                {

                }
            }
            cancellation.ThrowIfCancellationRequested();
        }

        private async Task SetWebHook()
        {
            SetWebhookRequest request = new SetWebhookRequest(_options.WebHookUrl)
            {
                SecretToken = _options.SecretToken,
                MaxConnections = _options.MaxConnections,
                DropPendingUpdates = _options.DropPendingUpdates,
                IpAddress = _options.IpAddress,
            };
            FileStream? fs = null;
            if (_options.CertificateFile != null)
            {
                FileInfo fileInfo = new FileInfo(_options.CertificateFile);
                fs = fileInfo.OpenRead();
                request.Certificate = new InputFileStream(fs, fileInfo.Name);
            }
            await _botClient.MakeRequestAsync(request);
            fs?.Dispose();
        }

        private async Task HandleContext(IUpdateHandler handler, HttpListenerContext context, CancellationToken cancellation)
        {
            if (context.Request.HttpMethod != "POST")
            {
                context.Response.StatusCode = 405;
                context.Response.StatusDescription = "Method Not Allowed";
                context.Response.Close();
                return;
            }

            if (!string.IsNullOrEmpty(_options.SecretToken))
            {
                string? secretTokenHeader = context.Request.Headers["X-Telegram-Bot-Api-Secret-Token"];
                if (secretTokenHeader != _options.SecretToken)
                {
                    _logger.LogWarning("Secret Token not contains: {0}", secretTokenHeader);
                    context.Response.StatusCode = 401;
                    context.Response.StatusDescription = "Unauthorized";
                    context.Response.Close();
                    return;
                }
            }
            using StreamReader sr = new StreamReader(context.Request.InputStream);
            string str = await sr.ReadToEndAsync();
            Update? update = JsonConvert.DeserializeObject<Update>(str);
            context.Response.StatusCode = 200;
            context.Response.StatusDescription = "OK";
            context.Response.Close();
            if (update != null)
                await handler.HandleUpdateAsync(_botClient, update, cancellation);
        }
    }
}
