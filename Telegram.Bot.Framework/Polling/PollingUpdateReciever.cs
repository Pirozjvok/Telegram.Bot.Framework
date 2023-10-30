using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Polling
{
    internal class PollingUpdateReciever : IUpdateReciever
    {
        private ITelegramBotClient _telegramBotClient;

        private ILogger<PollingUpdateReciever> _logger;

        private PollingOptions _pollingOptions;

        public PollingUpdateReciever(ITelegramBotClient telegramBotClient, ILogger<PollingUpdateReciever> logger, IOptions<PollingOptions> options)
        {
            _telegramBotClient = telegramBotClient;
            _logger = logger;
            _pollingOptions = options.Value;
        }

        public async Task RecieveAsync(IUpdateHandler handler, CancellationToken cancellation)
        {
            var updateHandlerAdapter = new PollingUpdateHandlerAdapter(_logger, handler);
            var receiverOptions = new Telegram.Bot.Polling.ReceiverOptions()
            {
                Limit = _pollingOptions.Limit,
                Offset = _pollingOptions.Offset,
                ThrowPendingUpdates = _pollingOptions.ThrowPendingUpdates,
            };
            var defaultUpdateReceiver = new Telegram.Bot.Polling.DefaultUpdateReceiver(_telegramBotClient, receiverOptions);
            await RemoveWebHook(cancellation);
            await defaultUpdateReceiver.ReceiveAsync(updateHandlerAdapter, cancellation);
        }

        private async Task RemoveWebHook(CancellationToken cancellationToken)
        {
            await _telegramBotClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
        }

        private class PollingUpdateHandlerAdapter : Telegram.Bot.Polling.IUpdateHandler
        {
            private ILogger<PollingUpdateReciever> _logger;

            private IUpdateHandler _updateHandler;

            public PollingUpdateHandlerAdapter(ILogger<PollingUpdateReciever> logger, IUpdateHandler updateHandler)
            {
                _logger = logger;
                _updateHandler = updateHandler;
            }

            public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                _logger.LogError(exception, "Polling error: {0}", exception.Message);
                return Task.CompletedTask;
            }

            public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                Task.Run(() => _updateHandler.HandleUpdateAsync(botClient, update, cancellationToken));
                return Task.CompletedTask;
            }
        }
    }
}
