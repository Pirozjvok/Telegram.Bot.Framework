using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace Telegram.Bot.Framework.FSM
{
    public class SqliteStateStorage : IStateStorage
    {
        private string _connectionString;

        private SqliteStateStorageOptions _options;

        public SqliteStateStorage(IOptions<SqliteStateStorageOptions> options)
        {
            _options = options.Value;
            _connectionString = CreateSqliteConnectionString();
            CheckDataBase();
        }

        public Task<bool> ContainsKeyAsync(long userId, string key)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @$"SELECT 1 FROM Datas WHERE UserId = $id AND Key = $key";
                command.Parameters.AddWithValue("$id", userId);
                command.Parameters.AddWithValue("$key", key);
                using (var reader = command.ExecuteReader())
                {
                    return Task.FromResult(reader.Read());
                }
            }
        }

        public Task<object?> GetUserDataAsync(long userId, string key, Type type)
        {
            using (var connection = CreateConnection())
            { 
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @$"SELECT Data FROM Datas WHERE UserId = $id AND Key = $key";
                command.Parameters.AddWithValue("$id", userId);
                command.Parameters.AddWithValue("$key", key);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string state = reader.GetString(0);
                        if (state == null)
                            return Task.FromResult<object?>(null);
                        return Task.FromResult(JsonSerializer.Deserialize(state, type));
                    }
                }
            }
            return Task.FromResult<object?>(null);
        }

        public Task<string?> GetUserStateAsync(long userId)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @$"SELECT State FROM States WHERE UserId = $id;";
                command.Parameters.AddWithValue("$id", userId);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return Task.FromResult<string?>(reader.GetString(0));
                    }
                }
            }
            return Task.FromResult<string?>(null);
        }

        public Task RemoveUserDataAsync(long userId, string key)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @$"DELETE FROM Datas WHERE UserId = $id AND Key = $key";
                command.Parameters.AddWithValue("$id", userId);
                command.Parameters.AddWithValue("$key", key);
                command.ExecuteNonQuery();
            }
            return Task.CompletedTask;
        }

        public Task SetUserDataAsync(long userId, string key, object? data)
        {
            if (data == null)
            {
                return RemoveUserDataAsync(userId, key);
            }
            else
            {
                using (var connection = CreateConnection())
                {
                    string json = JsonSerializer.Serialize(data);
                    connection.Open();
                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = @$"INSERT OR REPLACE INTO Datas VALUES ($id, $key, $value);";
                    command.Parameters.AddWithValue("$id", userId);
                    command.Parameters.AddWithValue("$key", key);
                    command.Parameters.AddWithValue("$value", json);
                    command.ExecuteNonQuery();
                }
            }
            return Task.CompletedTask;
        }

        public Task SetUserStateAsync(long userId, string? state)
        {
            using (var connection = CreateConnection())
            {
                connection.Open();

                if (state == null)
                {

                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = @$"DELETE FROM States WHERE UserId = $id;";
                    command.Parameters.AddWithValue("$id", userId);
                    command.ExecuteNonQuery();
                }
                else
                {

                    SqliteCommand command = connection.CreateCommand();
                    command.CommandText = @$"INSERT OR REPLACE INTO States VALUES ($id, $state);";
                    command.Parameters.AddWithValue("$id", userId);
                    command.Parameters.AddWithValue("$state", state);
                    command.ExecuteNonQuery();
                }
            }
            return Task.CompletedTask;
        }

        private string CreateSqliteConnectionString()
        {
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder()
            {
                Cache = SqliteCacheMode.Shared,
                DataSource = _options.DataSource,
                Pooling = true,
                Mode = SqliteOpenMode.ReadWriteCreate
            };
            return builder.ToString();
        }

        private SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        private void CheckDataBase()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var create = connection.CreateCommand();
                create.CommandText =
                @"
                    CREATE TABLE IF NOT EXISTS ""Datas"" (
                    ""UserId"" INTEGER NOT NULL,
                    ""Key""	 TEXT NOT NULL,
                    ""Data""	 TEXT,
                    PRIMARY KEY(""Key""));
                    CREATE TABLE IF NOT EXISTS ""States"" (
                    ""UserId"" INTEGER NOT NULL UNIQUE,
                    ""State""	 TEXT,
                    PRIMARY KEY(""UserId""));
                    CREATE INDEX IF NOT EXISTS ""DataKeys"" ON ""Datas"" (
                    ""UserId"",
                    ""Key"");
                ";
                create.ExecuteNonQuery();
            }
        }
    }
}
