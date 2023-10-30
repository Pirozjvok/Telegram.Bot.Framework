using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Telegram.Bot.Framework.FSM
{
    public class MemoryStateStorage : IStateStorage
    {
        private ConcurrentDictionary<long, string> _stateStorage = new();

        private ConcurrentDictionary<long, ConcurrentDictionary<string, object?>> _dataStorage = new();

        public string? GetUserState(long userId)
        {
            if (_stateStorage.TryGetValue(userId, out var state))
            {
                return state;
            }
            return null;
        }

        public void SetUserState(long userId, string? state)
        {
            if (state == null)
            {
                _stateStorage.TryRemove(userId, out _);
            } 
            else
            {
                _stateStorage[userId] = state;
            }        
        }

        public Task<string?> GetUserStateAsync(long userId) 
            => Task.FromResult(GetUserState(userId));

        public Task SetUserStateAsync(long userId, string? state)
        {
            SetUserState(userId, state);
            return Task.CompletedTask;
        }

        public object? GetUserData(long userId, string key)
        {
            CheckUser(userId);
            if (_dataStorage.TryGetValue(userId, out ConcurrentDictionary<string, object?>? _dataDict))
            {
                if (_dataDict.TryGetValue(key, out object? data))
                {
                    return data;
                }
            }
            return null;
        }

        public void RemoveUserData(long userId, string key)
        {
            CheckUser(userId);
            _dataStorage[userId].TryRemove(key, out _);
        }

        public void SetUserData(long userId, string key, object? data)
        {
            CheckUser(userId);
            _dataStorage[userId][key] = data;
        }

        public bool ContainsKey(long userId, string key)
        {
            CheckUser(userId);
            return _dataStorage[userId].ContainsKey(key);
        }

        public Task<object?> GetUserDataAsync(long userId, string key, Type type)
            => Task.FromResult(GetUserData(userId, key));

        public Task RemoveUserDataAsync(long userId, string key)
        {
            RemoveUserData(userId, key);
            return Task.CompletedTask;
        }

        public Task SetUserDataAsync(long userId, string key, object? data)
        {
            SetUserData(userId, key, data);
            return Task.CompletedTask;
        }

        public Task<bool> ContainsKeyAsync(long userId, string key) =>
            Task.FromResult(ContainsKey(userId, key));

        private void CheckUser(long userId)
        {
            if (!_dataStorage.ContainsKey(userId))
                _dataStorage.TryAdd(userId, new ConcurrentDictionary<string, object?>());
        }
    }
}
