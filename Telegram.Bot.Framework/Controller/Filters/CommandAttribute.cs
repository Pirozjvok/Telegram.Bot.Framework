using System;
using Telegram.Bot.Framework.Filter;

namespace Telegram.Bot.Framework.Controller
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAttribute : FilterAttribute
    {
        public override IUpdateFilter Filter { get; protected set; }

        public CommandAttribute(string command)
        {
            Filter = new CommandFilter(command);
        }
    }
}
