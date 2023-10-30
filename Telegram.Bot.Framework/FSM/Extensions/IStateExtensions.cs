using System.Threading.Tasks;

namespace Telegram.Bot.Framework.FSM
{
    public static class IStateExtensions
    {
        public static async Task<T?> GetUserDataAsync<T>(this IStateStorage stateStorage, long userId, string key)
        {
            return (T?)await stateStorage.GetUserDataAsync(userId, key, typeof(T));
        }
    }
}
