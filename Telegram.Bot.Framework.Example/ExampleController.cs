using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.FSM;
using Microsoft.Extensions.Logging;

namespace Telegram.Bot.Framework.Example
{
    internal class ExampleController : ControllerBase
    {
        [Command("/data set")]
        public async Task<string> DataSet(IStateStorage stateStorage)
        {
            await SetUserState("data");
            return "Enter data:";
        }

        [Command("/data")]
        public async Task<string> Data(IStateStorage stateStorage)
        {
            string? data = await stateStorage.GetUserDataAsync<string>(User!.Id, "data");
            return $"Data: {data}";
        }

        [State("data")]
        public async Task<string> OnData(IStateStorage stateStorage)
        {
            await SetUserState(null);
            await stateStorage.SetUserDataAsync(User!.Id, "data", Message?.Text);
            return "Data set";
        }

        [Command("/help")]
        [Command("help")]
        public async Task Help(ILogger<ExampleController> logger)
        {
            long userId = Message!.From!.Id;
            logger.LogWarning("User: {0} help!", Message!.From!.Id);
            await Client.SendTextMessageAsync(userId, "Help ok");
        }

        [Text("Hello")]
        [Text("Hello!")]
        public string Testing()
        {
            return "Hi";
        }

        [Command("/test")]
        public async Task<string> ToTest(IStateStorage stateStorage)
        {
            long userId = Message!.From!.Id;
            await stateStorage.SetUserStateAsync(userId, "test");
            return "Your state switched to \'test\'";
        }

        [State("test")]
        public async Task Test()
        {
            long userId = Message!.From!.Id;
            await Client.SendTextMessageAsync(userId, "Test ok");
        }
    }
}