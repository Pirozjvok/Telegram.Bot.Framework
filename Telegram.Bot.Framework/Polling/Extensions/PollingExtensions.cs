using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Polling;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Extensions
{
    public static class PollingExtensions
    {
        public static IServiceCollection UsePolling(this IServiceCollection serviceCollection, Action<PollingOptions>? configure = null)
        {
            var builder = serviceCollection
                .AddOptions<PollingOptions>()
                .BindConfiguration(nameof(PollingOptions));
            if (configure != null)
                builder.Configure(configure);
            return serviceCollection.AddSingleton<IUpdateReciever, PollingUpdateReciever>();
        }
    }
}
