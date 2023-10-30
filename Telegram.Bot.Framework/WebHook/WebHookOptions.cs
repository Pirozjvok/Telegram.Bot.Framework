namespace Telegram.Bot.Framework.WebHook
{
    public class WebHookOptions
    {
        #pragma warning disable CS8618
        public string ListenUrl { get; set; }

        public string WebHookUrl { get; set; }

        public string? IpAddress { get; set; }

        public string? CertificateFile { get; set; }

        public string? AllowedUpdates { get; set; }

        public bool DropPendingUpdates { get; set; }

        public string? SecretToken { get; set; }

        public int? MaxConnections { get; set; }
    }
}
