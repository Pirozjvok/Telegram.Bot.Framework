using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Framework.Handler;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Framework.Filter;

namespace Telegram.Bot.Framework.Example
{
    internal class StickerHandler : IHandler
    {
        public IUpdateFilter Filter { get; } = new AllFilter(new MessageTypeFilter(MessageType.Sticker), new StateFilter("echo.on"));

        public async Task Invoke(IUpdateContext context)
        {
            Message message = context.Update.Message!;
            await context.Client.SendStickerAsync(message.Chat.Id, InputFile.FromFileId(message.Sticker!.FileId));
        }
    }
}