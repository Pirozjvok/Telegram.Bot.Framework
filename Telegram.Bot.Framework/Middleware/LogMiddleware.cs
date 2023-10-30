using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Middleware
{
    public class LogMiddleware : IMiddleware
    {
        private ILogger<LogMiddleware> _logger;

        public LogMiddleware(ILogger<LogMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(IUpdateContext context, UpdateDelegate next)
        {
            LogUpdate(context.Update);
            try
            {
                await next.Invoke(context);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Middleware error: {message}", ex.Message);
            }        
        }

        private void LogUpdate(Update update)
        {
            switch (update.Type)
            {
                case UpdateType.Unknown:
                    break;
                case UpdateType.Message:
                    LogMessage(update.Message!);
                    break;
                case UpdateType.InlineQuery:
                    break;
                case UpdateType.ChosenInlineResult:
                    break;
                case UpdateType.CallbackQuery:
                    break;
                case UpdateType.EditedMessage:
                    break;
                case UpdateType.ChannelPost:
                    break;
                case UpdateType.EditedChannelPost:
                    break;
                case UpdateType.ShippingQuery:
                    break;
                case UpdateType.PreCheckoutQuery:
                    break;
                case UpdateType.Poll:
                    break;
                case UpdateType.PollAnswer:
                    break;
                case UpdateType.MyChatMember:
                    break;
                case UpdateType.ChatMember:
                    break;
                case UpdateType.ChatJoinRequest:
                    break;
            }
        }

        private void LogMessage(Message message)
        {
            switch (message.Type)
            {
                case MessageType.Unknown:
                    break;
                case MessageType.Text:
                    _logger.LogInformation("Message from: {userName}, id: {id}, text: {text}", '@' + message.From?.Username, message.From?.Id, message.Text);
                    break;
                case MessageType.Photo:
                    break;
                case MessageType.Audio:
                    break;
                case MessageType.Video:
                    break;
                case MessageType.Voice:
                    break;
                case MessageType.Document:
                    break;
                case MessageType.Sticker:
                    _logger.LogInformation("Sticker from: {userName}, id: {id}, fileId: {file}", '@' + message.From?.Username, message.From?.Id, message.Sticker?.FileId);
                    break;
                case MessageType.Location:
                    break;
                case MessageType.Contact:
                    break;
                case MessageType.Venue:
                    break;
                case MessageType.Game:
                    break;
                case MessageType.VideoNote:
                    break;
                case MessageType.Invoice:
                    break;
                case MessageType.SuccessfulPayment:
                    break;
                case MessageType.WebsiteConnected:
                    break;
                case MessageType.ChatMembersAdded:
                    break;
                case MessageType.ChatMemberLeft:
                    break;
                case MessageType.ChatTitleChanged:
                    break;
                case MessageType.ChatPhotoChanged:
                    break;
                case MessageType.MessagePinned:
                    break;
                case MessageType.ChatPhotoDeleted:
                    break;
                case MessageType.GroupCreated:
                    break;
                case MessageType.SupergroupCreated:
                    break;
                case MessageType.ChannelCreated:
                    break;
                case MessageType.MigratedToSupergroup:
                    break;
                case MessageType.MigratedFromGroup:
                    break;
                case MessageType.Poll:
                    break;
                case MessageType.Dice:
                    break;
                case MessageType.MessageAutoDeleteTimerChanged:
                    break;
                case MessageType.ProximityAlertTriggered:
                    break;
                case MessageType.WebAppData:
                    break;
                case MessageType.VideoChatScheduled:
                    break;
                case MessageType.VideoChatStarted:
                    break;
                case MessageType.VideoChatEnded:
                    break;
                case MessageType.VideoChatParticipantsInvited:
                    break;
                case MessageType.Animation:
                    break;
                case MessageType.ForumTopicCreated:
                    break;
                case MessageType.ForumTopicClosed:
                    break;
                case MessageType.ForumTopicReopened:
                    break;
                case MessageType.ForumTopicEdited:
                    break;
                case MessageType.GeneralForumTopicHidden:
                    break;
                case MessageType.GeneralForumTopicUnhidden:
                    break;
                case MessageType.WriteAccessAllowed:
                    break;
                case MessageType.UserShared:
                    break;
                case MessageType.ChatShared:
                    break;
            }
        }
    }
}
