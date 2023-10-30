using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Telegram.Bot.Framework.Bot;

namespace Telegram.Bot.Framework.Extensions
{
    public static class TelegramBotExtensions
    {
        public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection, Action<TelegramBotOptions>? configure = null)
        {
            return serviceCollection
                .AddTelegramBotOptions(configure)
                .AddSingleton<ITelegramBotClient, TelegramBotClient>(services => 
                {
                    TelegramBotOptions options = services.GetRequiredService<IOptions<TelegramBotOptions>>().Value;
                    return new TelegramBotClient(new TelegramBotClientOptions(options.Token, options.BaseUrl, options.UseTestEnvironment));
                });
        }

        public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection, ITelegramBotClient instance)
        {
            return serviceCollection
                .AddSingleton(instance);
        }

        internal static IServiceCollection AddTelegramBotOptions(this IServiceCollection serviceCollection, Action<TelegramBotOptions>? configure = null)
        {
            var builder = serviceCollection
                .AddOptions<TelegramBotOptions>()
                .BindConfiguration(nameof(TelegramBotOptions));
            if (configure != null)
                builder.Configure(configure);
            return serviceCollection;
        }
    }
}
