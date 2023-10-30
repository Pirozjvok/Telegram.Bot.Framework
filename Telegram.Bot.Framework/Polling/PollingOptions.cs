namespace Telegram.Bot.Framework.Polling
{
    public class PollingOptions
    {
        public bool ThrowPendingUpdates { get; set; }

        public int? Offset { get; set; }

        public int? Limit { get; set; }

        public PollingOptions()
        {
            Offset = null;
            Limit = null;
            ThrowPendingUpdates = false;
        }
    }
}
