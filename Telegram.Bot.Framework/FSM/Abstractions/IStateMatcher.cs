namespace Telegram.Bot.Framework.FSM
{
    public interface IStateMatcher
    {
        bool IsMatch(string? current, string? pattern);
    }
}
