using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Payments;
using Telegram.Bot.Types;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Framework.FSM;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller
{
    public abstract class ControllerBase
    {
        #pragma warning disable CS8618
        protected internal IUpdateContext Context { get; set; }
        
        protected IServiceProvider Services => Context.Services;

        protected ITelegramBotClient Client => Context.Client;

        protected UpdateType UpdateType => Context.Update.Type;

        protected Update Update => Context.Update;

        protected Message? Message => Update.Message;

        protected Message? EditedChannelPost => Update.EditedChannelPost;

        protected Message? ChannelPost => Update.ChannelPost;

        protected Message? EditedMessage => Update.EditedMessage;

        protected Poll? Poll => Update.Poll;

        protected PollAnswer? PollAnswer => Update.PollAnswer;

        protected PreCheckoutQuery? PreCheckoutQuery => Update.PreCheckoutQuery;

        protected CallbackQuery? CallbackQuery => Update.CallbackQuery;

        protected InlineQuery? InlineQuery => Update.InlineQuery;

        protected ChatJoinRequest? ChatJoinRequest => Update.ChatJoinRequest;

        protected ChatMemberUpdated? ChatMember => Update.ChatMember;

        protected ChatMemberUpdated? MyChatMember => Update.MyChatMember;

        protected ChosenInlineResult? ChosenInlineResult => Update.ChosenInlineResult;

        protected ShippingQuery? ShippingQuery => Update.ShippingQuery;

        protected User? User => Update.GetUser();

        protected Task SetUserState(string? state)
        {
            if (User != null)
                return GetStateStorage()?
                    .SetUserStateAsync(User.Id, state) ?? Task.CompletedTask;
            return Task.CompletedTask;
        }

        protected Task SetUserData(string key, object? data)
        {
            if (User != null)
                return GetStateStorage()?
                    .SetUserDataAsync(User.Id, key, data) ?? Task.CompletedTask;
            return Task.CompletedTask;
        }

        protected Task RemoveUserData(string key)
        {
            if (User != null)
                return GetStateStorage()?
                    .RemoveUserDataAsync(User.Id, key) ?? Task.CompletedTask;
            return Task.CompletedTask;
        }

        protected Task<T?> GetUserData<T>(string key)
        {
            if (User != null)
                return GetStateStorage()?
                    .GetUserDataAsync<T>(User.Id, key) ?? Task.FromResult<T?>(default);
            return Task.FromResult<T?>(default);
        }

        private IStateStorage? GetStateStorage() => Services.GetService<IStateStorage>();
    }
}
