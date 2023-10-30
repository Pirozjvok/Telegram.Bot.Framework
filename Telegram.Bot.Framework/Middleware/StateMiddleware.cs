using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.FSM;
using Telegram.Bot.Framework.Updates;
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
            long userId = 0;
            if (context.Update.Type == UpdateType.Message)
            {
                userId = context.Update.Message!.From!.Id;
            }
            string? state = null;
            if (userId != 0)
            {
                state = await _stateStorage.GetUserStateAsync(userId);
            }
            context.Items.Add(ContextItemName, state);
            await next(context);
        }
    }
}
