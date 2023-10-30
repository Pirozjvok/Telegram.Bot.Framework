using System;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Extensions
{
    public static class MapWhenExtensions
    {
        public static IPipelineBuilder MapWhen(this IPipelineBuilder builder, Func<IUpdateContext, bool> predicate, Action<IPipelineBuilder> configuration)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var branchBuilder = new PipelineBuilder(builder.Services);
            configuration(branchBuilder);
            var branch = branchBuilder.BuildPipeline();

            return builder.Use(_next =>
            {
                return context =>
                {
                    if (context == null)
                    {
                        throw new ArgumentNullException(nameof(context));
                    }

                    if (predicate!(context))
                    {
                        return branch!(context);
                    }
                    return _next(context);
                };
            });
        }
    }
}
