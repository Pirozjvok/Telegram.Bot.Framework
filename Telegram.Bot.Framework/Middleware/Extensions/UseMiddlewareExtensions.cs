using System;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Extensions;

namespace Telegram.Bot.Framework.Middleware
{
    public static class UseMiddlewareExtensions
    {
        public static IPipelineBuilder UseMiddleware<TMiddleware>(this IPipelineBuilder builder, params object?[] args) where TMiddleware : IMiddleware
        {
            return builder.Use((context, next) =>
            {
                IMiddleware middleware = (IMiddleware)ActivatorUtilities.CreateInstance(context.Services, typeof(TMiddleware), args!);
                return middleware.Invoke(context, next);
            });
        }
        public static IPipelineBuilder UseMiddleware<TMiddleware>(this IPipelineBuilder builder, Func<IServiceProvider, TMiddleware> factory) where TMiddleware : IMiddleware
        {
            return builder.Use((context, next) =>
            {
                IMiddleware middleware = factory(context.Services);
                return middleware.Invoke(context, next);
            });
        }
    }
}
