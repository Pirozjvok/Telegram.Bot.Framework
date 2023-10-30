using System.Threading.Tasks;
using Telegram.Bot.Framework.Filter;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Handler
{
    public interface IHandler
    {
        IUpdateFilter Filter { get; }

        Task Invoke(IUpdateContext context);
    }
}
