using System;
using Telegram.Bot.Framework.Filter;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class UpdateTypeAttribute : FilterAttribute
    {
        public override IUpdateFilter Filter { get; protected set; }

        public UpdateTypeAttribute(params UpdateType[] updateTypes)
        {
            Filter = new UpdateTypeFilter(updateTypes);
        }
    }
}
