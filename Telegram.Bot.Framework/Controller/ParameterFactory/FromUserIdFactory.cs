using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Abstractions;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Controller.ParameterFactory
{
    public class FromUserIdFactory : IParameterFactory
    {
        public bool IsAsync => false;

        public object? Create(IUpdateContext updateContext)
        {
            return updateContext.Update?.GetUser()?.Id;
        }

        public Task<object?> CreateAsync(IUpdateContext updateContext)
        {
            return Task.FromResult(Create(updateContext));
        }
    }
}
