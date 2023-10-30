using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Pipeline
{
    public class PipelineBuilder : IPipelineBuilder
    {
        private readonly List<Func<UpdateDelegate, UpdateDelegate>> _pipeline = new();

        public IServiceProvider Services { get; }

        public PipelineBuilder(IServiceProvider servicess)
        {
            Services = servicess;
        }

        public IPipelineBuilder Use(Func<UpdateDelegate, UpdateDelegate> updateDelegate)
        {
            _pipeline.Add(updateDelegate);
            return this;
        }

        public UpdateDelegate BuildPipeline()
        {
            UpdateDelegate last = context => Task.CompletedTask;
            for (var c = _pipeline.Count - 1; c >= 0; c--)
            {
                last = _pipeline[c](last);
            }
            return last;
        }
    }
}
