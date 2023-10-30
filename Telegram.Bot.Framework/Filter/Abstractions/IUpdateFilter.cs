using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Filter
{
    public interface IUpdateFilter
    {
        bool Check(IUpdateContext update);
    }
}
