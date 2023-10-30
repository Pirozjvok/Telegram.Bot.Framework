using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Filter
{
    public class AnyFilter : IUpdateFilter
    {
        private IUpdateFilter[] _filters;

        public AnyFilter(params IUpdateFilter[] filters)
        {
            _filters = filters;
        }

        public bool Check(IUpdateContext item)
        {
            bool state = false;
            for (int i = 0; i < _filters.Length; i++)
            {
                if (_filters[i].Check(item) is true)
                {
                    state = true;
                    break;
                }
            }
            return state;
        }
    }
}
