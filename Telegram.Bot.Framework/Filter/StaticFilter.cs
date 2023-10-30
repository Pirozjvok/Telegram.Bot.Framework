using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Filter
{
    public class StaticFilter : IUpdateFilter
    {
        private bool _value;

        public StaticFilter(bool value)
        {
            _value = value;
        }

        public bool Check(IUpdateContext item) => _value;
    }
}
