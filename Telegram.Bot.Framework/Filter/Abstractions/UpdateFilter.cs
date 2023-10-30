using System;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Filter.Abstractions
{
    public class UpdateFilter : IUpdateFilter
    {
        private Predicate<IUpdateContext> _predicate;

        public UpdateFilter(Predicate<IUpdateContext> predicate)
        {
            _predicate = predicate;
        }

        public bool Check(IUpdateContext update)
        {
            return _predicate.Invoke(update);
        }
    }
}
