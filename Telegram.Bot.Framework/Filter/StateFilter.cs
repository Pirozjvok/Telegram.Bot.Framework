using Telegram.Bot.Framework.FSM;
using Telegram.Bot.Framework.Middleware;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Filter
{
    public class StateFilter : IUpdateFilter
    {
        private IStateMatcher _stateMatcher;

        private string? _state;

        public StateFilter(string? state)
        {
            _stateMatcher = new StateMatcher();
            _state = state;
        }

        public bool Check(IUpdateContext item)
        {
            string? current = item.Items[StateMiddleware.ContextItemName] as string;
            return _stateMatcher.IsMatch(current, _state);
        }
    }
}
