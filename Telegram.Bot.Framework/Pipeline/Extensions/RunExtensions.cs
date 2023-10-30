using System;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Extensions
{
    public static class RunExtensions
    {
        public static void Run(this IPipelineBuilder builder, UpdateDelegate handler)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            builder.Use(_ => handler);
        }
    }
}
