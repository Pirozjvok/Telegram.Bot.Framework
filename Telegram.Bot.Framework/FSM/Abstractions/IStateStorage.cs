using System;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.FSM
{
    public interface IStateStorage
    {
        Task<string?> GetUserStateAsync(long userId);

        Task SetUserStateAsync(long userId, string? state);

        Task<object?> GetUserDataAsync(long userId, string key, Type type);

        Task SetUserDataAsync(long userId, string key, object? data);

        Task RemoveUserDataAsync(long userId, string key);

        Task<bool> ContainsKeyAsync(long userId, string key);
    }
}
