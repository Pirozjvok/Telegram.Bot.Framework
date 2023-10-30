using System;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Filter
{
    public class CommandFilter : IUpdateFilter
    {
        private string _command;

        public CommandFilter(string command) 
        { 
            _command = command;
        }

        public bool Check(IUpdateContext item)
        {
            if (item.Update.Type == UpdateType.Message && item.Update?.Message?.Type == MessageType.Text)
            {
                string? text = item.Update.Message.Text;
                if (text == null)
                    return false;
                bool startWith = text.StartsWith(_command, StringComparison.OrdinalIgnoreCase);
                return startWith && (_command.Length == text.Length || text[_command.Length] == ' ');
            }
            return false;
        }
    }
}
