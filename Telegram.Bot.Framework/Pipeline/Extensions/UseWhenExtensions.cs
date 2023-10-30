using System;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Extensions
{
    public static class UseWhenExtensions
    {
        public static IPipelineBuilder UseWhen(this IPipelineBuilder builder, Func<IUpdateContext, bool> predicate, Action<IPipelineBuilder> configuration)
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

            return builder.Use(main =>
            {
                branchBuilder.Run(main);
                var branch = branchBuilder.BuildPipeline();

                return context =>
                {
                    if (predicate(context))
                    {
                        return branch(context);
                    }
                    else
                    {
                        return main(context);
                    }
                };
            });
        }
    }
}