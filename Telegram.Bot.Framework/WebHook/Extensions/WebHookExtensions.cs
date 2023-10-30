using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Framework.WebHook;

namespace Telegram.Bot.Framework.Extensions
{
    public static class WebHookExtensions
    {
        public static IServiceCollection UseWebHook(this IServiceCollection serviceCollection, Action<WebHookOptions>? configure = null)
        {
            var builder = serviceCollection
                .AddOptions<WebHookOptions>()
                .BindConfiguration(nameof(WebHookOptions));
            if (configure != null)
                builder.Configure(configure);
            return serviceCollection.AddSingleton<IUpdateReciever, WebHookUpdateReciever>();
        }
    }
}
