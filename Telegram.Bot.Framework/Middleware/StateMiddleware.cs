using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.FSM;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Middleware
{
    public class StateMiddleware : IMiddleware
    {
        public const string ContextItemName = "__State";

        private IStateStorage _stateStorage;

        public StateMiddleware(IStateStorage stateStorage)
        {
            _stateStorage = stateStorage;
        }

        public async Task Invoke(IUpdateContext context, UpdateDelegate next)
        {
            User? user = context.Update.GetUser();
            string? state = null;
            if (user != null)
            {
                state = await _stateStorage.GetUserStateAsync(user.Id);
            }
            context.Items.Add(ContextItemName, state);
            await next(context);
        }
    }
}
