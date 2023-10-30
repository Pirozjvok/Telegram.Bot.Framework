using Microsoft.Extensions.Hosting;
using Telegram.Bot.Framework.Application;
using Telegram.Bot;
using Telegram.Bot.Framework.Extensions;
using Telegram.Bot.Framework.FSM;
using Telegram.Bot.Framework.Middleware;
using Telegram.Bot.Framework.Handler;

namespace Telegram.Bot.Framework.Example
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = TelegramBotApplication.CreateBuilder(args);

            builder.Services.AddTelegramBotClient();
            builder.Services.AddSqliteStateStorage();
            builder.Services.UsePolling();

            var app = builder.Build();

            app.UseLogging();
            app.UseFSM();
            app.UseHandlers();
            app.UseControllers();
            app.Run();
        }
    }
}