using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Filter
{
    public class MessageTypeFilter : IUpdateFilter
    {
        private HashSet<MessageType> _messageTypes;

        public MessageTypeFilter(params MessageType[] messageTypes)
        {
            _messageTypes = new HashSet<MessageType>(messageTypes);
        }

        public bool Check(IUpdateContext update)
        {
            if (update.Update?.Message is not null)
            {
                return _messageTypes.Contains(update.Update.Message.Type);
            }
            return false;
        }
    }
}
