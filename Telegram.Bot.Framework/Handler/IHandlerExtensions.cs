using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegram.Bot.Framework.Extensions;
using Telegram.Bot.Framework.Pipeline;

namespace Telegram.Bot.Framework.Handler
{
    public static class IHandlerExtensions
    {
        public static IPipelineBuilder UseHandlers(this IPipelineBuilder pipelineBuilder)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            IHandler[] handlers = assembly
                .GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract && typeof(IHandler).IsAssignableFrom(x))
                .Select(x => (IHandler)ActivatorUtilities.CreateInstance(pipelineBuilder.Services, x))
                .ToArray();

            return pipelineBuilder.Use((context, next) =>
            {
                IHandler? handler = handlers.FirstOrDefault(x => x.Filter.Check(context));
                return handler is null ? next.Invoke(context) : handler.Invoke(context);
            });
        }

        public static IPipelineBuilder UseHandler(this IPipelineBuilder pipelineBuilder, Func<IServiceProvider, IHandler> factory)
        {
            return pipelineBuilder.Use((context, next) =>
            {
                IHandler handler = factory(context.Services);
                return handler.Filter.Check(context) ? handler.Invoke(context) : next(context);
            });
        }

        public static IPipelineBuilder UseHandler<THandler>(this IPipelineBuilder pipelineBuilder, params object?[] args)
        {
            return pipelineBuilder.UseHandler(services =>
            {
                return (IHandler)ActivatorUtilities.CreateInstance(services, typeof(THandler), args!);
            });
        }

        internal static IPipelineBuilder UseHandlers(this IPipelineBuilder pipelineBuilder, IHandler[] handlers)
        {
            return pipelineBuilder.Use((context, next) =>
            {
                IHandler? handler = handlers.FirstOrDefault(x => x.Filter.Check(context));
                return handler?.Invoke(context) ?? next.Invoke(context);
            });
        }
    }
}
