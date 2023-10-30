using Telegram.Bot.Framework.Updates;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controller.Abstractions
{
    public interface IParameterFactory
    {
        bool IsAsync { get; }

        object? Create(IUpdateContext updateContext);

        Task<object?> CreateAsync(IUpdateContext updateContext);
    }
}
