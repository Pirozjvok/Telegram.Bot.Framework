using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.FSM;

namespace Telegram.Bot.Framework.Example
{
    internal class EchoController : ControllerBase
    {
        [Command("/echo on")]
        public async Task<string> EchoOn(IStateStorage stateStorage)
        {
            await SetUserState("echo.on");
            return "Echo On";
        }

        [Command("/echo off")]
        [State("echo.on")]
        public async Task<string> EchoOff(IStateStorage stateStorage)
        {
            await SetUserState(null);
            return "Echo Off";
        }

        [State("echo.on")]
        public string Echo() => Message?.Text!;
    }
}