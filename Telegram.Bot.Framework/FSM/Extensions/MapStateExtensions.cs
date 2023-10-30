using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Extensions;
using Telegram.Bot.Framework.Middleware;
using Telegram.Bot.Framework.Pipeline;

namespace Telegram.Bot.Framework.FSM
{
    public static class MapStateExtensions
    {
        public static IPipelineBuilder MapState(this IPipelineBuilder builder, string? state, Action<IPipelineBuilder> configuration)
        {
            return builder.MapWhen(context =>
            {
                IStateMatcher stateMatcher = context.Services.GetRequiredService<IStateMatcher>();
                string? currentState = context.Items[StateMiddleware.ContextItemName] as string;
                return stateMatcher.IsMatch(currentState, state);
            }, configuration);
        }
    }
}
