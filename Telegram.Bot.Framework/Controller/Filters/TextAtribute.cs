using System;
using Telegram.Bot.Framework.Filter;

namespace Telegram.Bot.Framework.Controller
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TextAttribute : FilterAttribute
    {
        public override IUpdateFilter Filter { get; protected set; }

        public TextAttribute(string text)
        {
            Filter = new TextFilter(text);
        }
    }
}
