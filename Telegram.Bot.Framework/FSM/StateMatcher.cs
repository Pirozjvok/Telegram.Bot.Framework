using System;

namespace Telegram.Bot.Framework.FSM
{
    internal class StateMatcher : IStateMatcher
    {
        public bool IsMatch(string? current, string? pattern)
        {
            if (current == null && pattern == null)
                return true;
            if (current == null || pattern == null)
            {
                if (pattern == "*")
                    return true;
                return false;
            }
            string[] currentSplited = current.Split('.');
            string[] patternSplited = pattern.Split('.');
            int minLength = Math.Min(currentSplited.Length, patternSplited.Length);
            int i = 0;
            for (; i < minLength; i++)
            {
                if (currentSplited[i] != patternSplited[i])
                    break;
            }
            if (currentSplited.Length == patternSplited.Length && minLength == i)
                return true;
            if (patternSplited.Length - 1 == i && patternSplited[i] == "*" && currentSplited.Length > i)
                return true;
            return false;
        }
    }
}
