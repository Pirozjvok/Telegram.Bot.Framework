using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Types
{
    public static class UpdateExtensions
    {
        public static User? GetUser(this Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                    return null;
                case UpdateType.Message:
                    return update.Message?.From;
                case UpdateType.InlineQuery:
                    return update.InlineQuery?.From;
                case UpdateType.ChosenInlineResult:
                    return update.ChosenInlineResult?.From;
                case UpdateType.CallbackQuery:
                    return update.CallbackQuery?.From;
                case UpdateType.EditedMessage:
                    return update.EditedMessage?.From;
                case UpdateType.ChannelPost:
                    return update.ChannelPost?.From;
                case UpdateType.EditedChannelPost:
                    return update.EditedChannelPost?.From;
                case UpdateType.ShippingQuery:
                    return update.ShippingQuery?.From;
                case UpdateType.PreCheckoutQuery:
                    return update.PreCheckoutQuery?.From;
                case UpdateType.Poll:
                    return null;
                case UpdateType.PollAnswer:
                    return update.PollAnswer?.User;
                case UpdateType.MyChatMember:
                    return update.MyChatMember?.From;
                case UpdateType.ChatMember:
                    return update.ChatMember?.From;
                case UpdateType.ChatJoinRequest:
                    return update.ChatJoinRequest?.From;
                default:
                    return null;
            }

        }
    }
}
