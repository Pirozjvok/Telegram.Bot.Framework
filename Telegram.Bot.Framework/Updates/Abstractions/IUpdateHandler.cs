using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Updates
{
    public interface IUpdateHandler
    {
        Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken);
    }
}
