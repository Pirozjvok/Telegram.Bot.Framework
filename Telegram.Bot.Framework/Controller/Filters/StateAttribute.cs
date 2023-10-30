using System;
using Telegram.Bot.Framework.Filter;

namespace Telegram.Bot.Framework.Controller
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class StateAttribute : FilterAttribute
    {
        public StateAttribute(string state)
        {
            Filter = new StateFilter(state);
        }

        public override IUpdateFilter Filter { get; protected set; }
    }
}
