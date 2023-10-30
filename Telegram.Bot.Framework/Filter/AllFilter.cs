using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Filter
{
    public class AllFilter : IUpdateFilter
    {
        private IUpdateFilter[] _filters;

        public AllFilter(params IUpdateFilter[] filters)
        {
            _filters = filters;
        }

        public bool Check(IUpdateContext item)
        {
            bool state = true;
            for (int i = 0; i < _filters.Length; i++)
            {
                if (_filters[i].Check(item) is false)
                {
                    state = false;
                    break;
                }
            }
            return state;
        }
    }
}
