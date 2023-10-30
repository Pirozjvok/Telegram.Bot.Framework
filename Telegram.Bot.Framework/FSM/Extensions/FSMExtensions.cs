using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Extensions;
using Telegram.Bot.Framework.Middleware;
using Telegram.Bot.Framework.Pipeline;

namespace Telegram.Bot.Framework.FSM
{
    public static class FSMExtensions
    {
        public static IServiceCollection AddMemoryStateStorage(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddSingleton<IStateMatcher, StateMatcher>()
                .AddSingleton<IStateStorage, MemoryStateStorage>();
        }
            
        public static IServiceCollection AddSqliteStateStorage(this IServiceCollection serviceCollection, Action<SqliteStateStorageOptions>? configure = null)
        {
            var builder = serviceCollection
                .AddOptions<SqliteStateStorageOptions>()
                .BindConfiguration(nameof(SqliteStateStorageOptions));
            if (configure != null)
                builder.Configure(configure);
            return serviceCollection
                .AddSingleton<IStateMatcher, StateMatcher>()
                .AddSingleton<IStateStorage, SqliteStateStorage>();
        }

        public static IPipelineBuilder UseState(this IPipelineBuilder builder, string? state, Action<IPipelineBuilder> configuration)
        {
            return builder.UseWhen(context =>
            {
                IStateMatcher stateMatcher = context.Services.GetRequiredService<IStateMatcher>();
                string? currentState = context.Items[StateMiddleware.ContextItemName] as string;
                return stateMatcher.IsMatch(currentState, state);
            }, configuration);
        }          
    }
}
