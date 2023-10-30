using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Extensions
{
    public static class UseExtensions
    {
        public static IPipelineBuilder Use(this IPipelineBuilder builder, Func<IUpdateContext, UpdateDelegate, Task> middleware)
        {
            return builder.Use(next => context => middleware(context, next));
        }
    }
}
