using Telegram.Bot.Framework.Filter;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller
{
    public class MessageTypeAttribute : FilterAttribute
    {
        public override IUpdateFilter Filter { get; protected set; }

        public MessageTypeAttribute(params MessageType[] messageTypes)
        {
            Filter = new MessageTypeFilter(messageTypes);
        }
    }
}
