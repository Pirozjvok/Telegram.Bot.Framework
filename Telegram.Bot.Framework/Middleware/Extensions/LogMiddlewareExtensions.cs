using Telegram.Bot.Framework.Pipeline;

namespace Telegram.Bot.Framework.Middleware
{
    public static class LogMiddlewareExtensions
    {
        public static IPipelineBuilder UseLogging(this IPipelineBuilder builder)
        {
            return builder.UseMiddleware<LogMiddleware>();
        }
    }
}
