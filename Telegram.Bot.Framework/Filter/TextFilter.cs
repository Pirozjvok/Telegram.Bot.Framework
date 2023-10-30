using System;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Filter
{
    public class TextFilter : IUpdateFilter
    {
        private string _text;

        public TextFilter(string text)
        { 
            _text = text;
        }

        public bool Check(IUpdateContext item)
        {
            if (item.Update.Type == UpdateType.Message &&
                item.Update.Message != null)
            {
                string? text = item.Update.Message.Text;
                if (text == null)
                    return false;
                return text.Equals(_text, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }
    }
}
