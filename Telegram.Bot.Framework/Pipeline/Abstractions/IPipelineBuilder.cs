using System;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Pipeline
{
    public interface IPipelineBuilder
    {
        IServiceProvider Services { get; }

        IPipelineBuilder Use(Func<UpdateDelegate, UpdateDelegate> updateDelegate);

        UpdateDelegate BuildPipeline();
    }
}
