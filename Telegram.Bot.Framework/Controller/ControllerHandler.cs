using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Filter;
using Telegram.Bot.Framework.Handler;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Framework.Controller.Abstractions;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;
using Telegram.Bot.Framework.Controller.ParameterFactory;
using Telegram.Bot.Framework.Filter.Abstractions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Controller
{
    public class ControllerHandler : IHandler
    {
        private Func<IUpdateContext, Task<object?>> _invokeDelegate;

        public IUpdateFilter Filter { get; private set; }

        public ControllerHandler(MethodInfo methodInfo, Func<IServiceProvider, ControllerBase> factory)
        {
            List<IUpdateFilter> filters = new List<IUpdateFilter>();

            _invokeDelegate = CreateDelegate(methodInfo, factory, filters);

            filters.AddRange(methodInfo
                .GetCustomAttributes()
                .Where(attr => attr is FilterAttribute)
                .Cast<FilterAttribute>()
                .GroupBy(attr => attr.GetType())
                .Select(type => new AnyFilter(type.Select(attr => attr.Filter).ToArray())));

            Filter = filters.Count switch
            {
                0 => new StaticFilter(false),
                1 => filters.First(),
                _ => new AllFilter(filters.ToArray())
            };
        }

        public async Task Invoke(IUpdateContext context)
        {
            object? result = await _invokeDelegate.Invoke(context);

            switch (result)
            {
                case string msg:
                    User? user = context.Update.GetUser();
                    if (user == null)
                        break;
                    if (context.Update.Message != null && context.Update.Message.Chat.Id != user.Id)
                    {
                        await context.Client.SendTextMessageAsync(context.Update.Message.Chat.Id, msg, replyToMessageId: context.Update.Message.MessageId);
                    }
                    else
                    {
                        await context.Client.SendTextMessageAsync(user.Id, msg);
                    }                
                    break;
            }
        }

        private static Func<IUpdateContext, Task<object?>> CreateDelegate(MethodInfo methodInfo, Func<IServiceProvider, ControllerBase> factory, List<IUpdateFilter> filters)
        { 
            MethodInfo? getTaskResult = null;
            if (methodInfo.ReturnType.IsGenericType && methodInfo.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                getTaskResult = methodInfo.ReturnType.GetProperty("Result")!.GetGetMethod();
            }

            ParameterInfo[] parameters = methodInfo.GetParameters();
            IParameterFactory[] parameterFactories = new IParameterFactory[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterAttribute? attribute = parameters[i].GetCustomAttribute<ParameterAttribute>();
 
                if (attribute == null)
                {
                    switch (parameters[i].ParameterType.FullName)
                    {
                        case "System.String":
                            parameterFactories[i] = new FromMessageTextFactory();
                            filters.Add(new MessageTypeFilter(MessageType.Text));
                            break;
                        case "System.Int64":
                            parameterFactories[i] = new FromUserIdFactory();
                            filters.Add(new UpdateFilter(x => x.Update.GetUser() is not null));
                            break;
                        default:
                            parameterFactories[i] = new FromServicesFactory(parameters[i]);
                            break;
                    }                    
                } 
                else
                {
                    if (attribute.Filter != null)
                        filters.Add(attribute.Filter);
                    parameterFactories[i] = attribute.Factory.Invoke(parameters[i]);
                }
            }

            return async context =>
            {
                object?[] arguments = new object[parameterFactories.Length];

                for (int i = 0; i < arguments.Length; i++)
                {
                    IParameterFactory parameterFactory = parameterFactories[i];
                    if (parameterFactory.IsAsync)
                    {
                        arguments[i] = await parameterFactory.CreateAsync(context);
                    } 
                    else
                    {
                        arguments[i] = parameterFactory.Create(context);
                    }                  
                }

                ControllerBase controller = factory.Invoke(context.Services);
                controller.Context = context;

                object? result = methodInfo.Invoke(controller, arguments);

                if (result == null)
                    return null;

                if (result is Task task)
                    await task;

                if (getTaskResult != null)
                    return getTaskResult.Invoke(result, null);

                return result;
            };
        }
    }
}
