using Telegram.Bot.Framework.Pipeline;

namespace Telegram.Bot.Framework.Middleware
{
    public static class StateMiddlewareExtensions
    {
        public static IPipelineBuilder UseFSM(this IPipelineBuilder builder) =>
            builder.UseMiddleware<StateMiddleware>();
    }
}
