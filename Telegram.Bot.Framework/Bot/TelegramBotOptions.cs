namespace Telegram.Bot.Framework.Bot
{
    public class TelegramBotOptions
    {
        #pragma warning disable CS8618
        public string Token { get; set; }   

        public string? BaseUrl { get; set; }

        public bool UseTestEnvironment { get; set; }
    }
}
