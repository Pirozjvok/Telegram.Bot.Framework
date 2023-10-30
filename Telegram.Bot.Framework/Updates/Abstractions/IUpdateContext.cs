using System;
using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Updates
{
    public interface IUpdateContext
    {
        IServiceProvider Services { get; }

        ITelegramBotClient Client { get; }

        Update Update { get; }

        IDictionary<string, object?> Items { get; }
    }
}
