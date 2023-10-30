using System;
using Telegram.Bot.Framework.Filter;

namespace Telegram.Bot.Framework.Controller
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public abstract class FilterAttribute : Attribute
    {
        public abstract IUpdateFilter Filter { get; protected set; }
    }
}
