using System.Threading;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Updates
{
    public interface IUpdateReciever
    {
        Task RecieveAsync(IUpdateHandler handler, CancellationToken cancellation);
    }
}
