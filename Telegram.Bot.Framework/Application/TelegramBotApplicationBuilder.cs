using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Application
{
    public class TelegramBotApplicationBuilder
    {
        private HostApplicationBuilder _builder;

        public IServiceCollection Services => _builder.Services;

        public ILoggingBuilder Logging => _builder.Logging;

        public IHostEnvironment Environment => _builder.Environment;

        public ConfigurationManager Configuration => _builder.Configuration;

        private TelegramBotApplication? _builtApplication;

        internal TelegramBotApplicationBuilder(string[]? args)
        {
            _builder = Host.CreateApplicationBuilder(args);
            _builder.Services.AddHostedService(CreateRecieverService);
        }

        public void ConfigureContainer<TContainerBuilder>(
            IServiceProviderFactory<TContainerBuilder> factory,
            Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull
                => _builder.ConfigureContainer(factory, configure);

        public TelegramBotApplication Build()
        {
            _builtApplication = new TelegramBotApplication(_builder.Build());
            return _builtApplication;
        }

        private TelegramBotService CreateRecieverService(IServiceProvider serviceProvider)
        {
            UpdateDelegate updateDelegate = ((IPipelineBuilder)_builtApplication!).BuildPipeline();
            TelegramBotService recieverService = new TelegramBotService(updateDelegate, serviceProvider);
            return recieverService;
        }
    }
}
