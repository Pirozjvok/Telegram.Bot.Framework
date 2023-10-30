using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Abstractions;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Controller.ParameterFactory
{
    internal class FromMessageTextFactory : IParameterFactory
    {
        public bool IsAsync => false;

        public object? Create(IUpdateContext updateContext)
        {
            return updateContext.Update?.Message?.Text;
        }

        public Task<object?> CreateAsync(IUpdateContext updateContext)
        {
            return Task.FromResult(Create(updateContext));
        }
    }
}
