using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.FSM;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Framework.Controller.Abstractions;
using System.Reflection;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Controller
{
    internal class FromStorageFactory : IParameterFactory
    {
        private Type _parameterType;

        private string _parameterName;

        public FromStorageFactory(ParameterInfo parameterInfo)
        {
            _parameterName = parameterInfo.Name!;
            _parameterType = parameterInfo.ParameterType;          
        }

        public bool IsAsync => true;

        public object? Create(IUpdateContext updateContext)
        {
            return CreateAsync(updateContext).Result;
        }

        public async Task<object?> CreateAsync(IUpdateContext updateContext)
        {
            IStateStorage stateStorage = updateContext.Services.GetRequiredService<IStateStorage>();
            User? user = updateContext.Update.GetUser();
            if (user == null)
                return null;
            return await stateStorage.GetUserDataAsync(user.Id, _parameterName, _parameterType);
        }
    }
}
