using System;
using System.Linq;
using System.Reflection;
using Telegram.Bot.Framework.Handler;
using Telegram.Bot.Framework.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Controller;

namespace Telegram.Bot.Framework.Extensions
{
    public static class ControllerExtensions
    {
        public static IPipelineBuilder UseControllers(this IPipelineBuilder builder, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();
            IHandler[] handlers = assembly
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(ControllerBase).IsAssignableFrom(type))
                .SelectMany(type => GetHandlers(type, builder.Services, GetControllerFactory(type, builder.Services)))
                .ToArray();
            return builder.UseHandlers(handlers);
        }

        public static IPipelineBuilder UseController<TController>(this IPipelineBuilder builder, params object?[] parameters) where TController : ControllerBase
        {
            IHandler[] handlers = GetHandlers(builder.Services, GetControllerFactory(typeof(TController), builder.Services, parameters));
            return builder.UseHandlers(handlers);
        }

        public static IPipelineBuilder UseController<TController>(this IPipelineBuilder builder, Func<IServiceProvider, TController> factory) where TController : ControllerBase
        {
            IHandler[] handlers = GetHandlers(builder.Services, factory);
            return builder.UseHandlers(handlers);
        }

        private static IHandler[] GetHandlers<TController>(IServiceProvider services, Func<IServiceProvider, TController> factory) where TController : ControllerBase
        {
            Type type = typeof(TController);
            return GetHandlers(type, services, factory);
        }

        private static IHandler[] GetHandlers(Type type, IServiceProvider services, Func<IServiceProvider, ControllerBase> factory)
        {
            return type.GetMethods()
                .Where(x => x.IsPublic && !x.IsStatic && !x.IsAbstract)
                .Select(methodInfo => new ControllerHandler(methodInfo, p => factory(p)))
                .ToArray();
        }

        private static Func<IServiceProvider, ControllerBase> GetControllerFactory(Type type, IServiceProvider serviceProvider, params object?[] parameters)
        {
            return services => (ControllerBase)ActivatorUtilities.CreateInstance(services, type, parameters!);
        }
    }
}
