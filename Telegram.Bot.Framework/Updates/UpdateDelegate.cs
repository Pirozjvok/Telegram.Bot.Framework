using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Updates
{
    public delegate Task UpdateDelegate(IUpdateContext context);
}