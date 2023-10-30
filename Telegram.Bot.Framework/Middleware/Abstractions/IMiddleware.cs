using System.Threading.Tasks;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Middleware
{
    public interface IMiddleware
    {
        Task Invoke(IUpdateContext context, UpdateDelegate next);
    }
}
