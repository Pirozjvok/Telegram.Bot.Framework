using System;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Framework.Controller.Abstractions;

namespace Telegram.Bot.Framework.Controller
{
    internal class FromServicesFactory : IParameterFactory
    {
        private Type _parameterType;

        public FromServicesFactory(ParameterInfo parameterInfo)
        {
            _parameterType = parameterInfo.ParameterType;
        }

        public bool IsAsync => false;

        public object? Create(IUpdateContext updateContext)
        {
            return updateContext.Services.GetService(_parameterType);
        }

        public Task<object?> CreateAsync(IUpdateContext updateContext)
        {
            return Task.FromResult(Create(updateContext));
        }
    }
}
