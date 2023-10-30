using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Types;
using Microsoft.Extensions.Logging;

namespace Telegram.Bot.Framework.Application
{
    public class TelegramBotService : BackgroundService, IUpdateHandler
    {
        private UpdateDelegate _updateDelegate;

        private IUpdateReciever _updateReciever;

        private ILogger<TelegramBotService> _logger;

        private IServiceProvider _serviceProvider;

        public TelegramBotService(UpdateDelegate updateDelegate, IServiceProvider serviceProvider)
        {
            _updateDelegate = updateDelegate;
            _serviceProvider = serviceProvider;
            _updateReciever = serviceProvider.GetRequiredService<IUpdateReciever>();
            _logger = serviceProvider.GetRequiredService<ILogger<TelegramBotService>>();
        }

        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            UpdateContext updateContext = new UpdateContext(botClient, _serviceProvider, update);
            _logger.LogTrace("New update: {@Update}", update);
            return _updateDelegate.Invoke(updateContext);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _updateReciever.RecieveAsync(this, stoppingToken);
        }

        private class UpdateContext : IUpdateContext
        {
            public IServiceProvider Services { get; }

            public ITelegramBotClient Client { get; }

            public Update Update { get; }

            public IDictionary<string, object?> Items { get; }

            public UpdateContext(ITelegramBotClient client, IServiceProvider services, Update update)
            {
                Services = services;
                Client = client;
                Update = update;
                Items = new Dictionary<string, object?>();
            }
        }
    }
}
