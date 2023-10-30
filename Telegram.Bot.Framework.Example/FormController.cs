using Telegram.Bot.Types.Enums;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.FSM;
using System.Text;

namespace Telegram.Bot.Framework.Example
{
    internal class Form
    {
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? About { get; set; }
    }

    internal class FormController : ControllerBase
    {
        [Command("/form")]
        public async Task<string> StartForm()
        {
            await SetUserState("form.first_name");
            return "Ваше имя:";
        }

        [State("form.first_name")]
        public async Task<string> FormFirstName(string text)
        {
            Form form = new Form();
            form.FirstName = text;
            await SetUserState("form.second_name");
            await SetUserData("form", form);
            return "Ваша фамилия:";
        }

        [State("form.second_name")]
        public async Task FormSecondName(string text, long userId, IStateStorage stateStorage, [FromStorage] Form form)
        {
            form.SecondName = text;
            await stateStorage.SetUserStateAsync(userId, "form.about");
            await stateStorage.SetUserDataAsync(userId, "form", form);
            await Client.SendTextMessageAsync(userId, "О себе:");
        }

        [State("form.about")]
        [MessageType(MessageType.Text)]
        public async Task<string> FormAbout([FromStorage] Form form)
        {
            form.About = Message!.Text!;
            await SetUserState(null);
            await SetUserData("form", form);
            return "Успешно";
        }

        [Command("/getform")]
        public async Task<string> GetForm([FromStorage] Form? form)
        {
            if (form == null)
                return "Form is empty";
            await RemoveUserData("form");
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"First name: {form.FirstName}");
            stringBuilder.AppendLine($"Second name: {form.SecondName}");
            stringBuilder.AppendLine($"About: {form.About}");
            return stringBuilder.ToString();
        }
    }
}