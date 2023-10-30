using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Updates;

namespace Telegram.Bot.Framework.Application
{
    public class TelegramBotApplication : IPipelineBuilder, IHost
    {
        public static TelegramBotApplicationBuilder CreateBuilder(string[]? args)
            => new TelegramBotApplicationBuilder(args);

        private IHost _host;

        private PipelineBuilder _pipelineBuilder;

        public IServiceProvider Services => _host.Services;

        public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();

        public IHostEnvironment Environment => _host.Services.GetRequiredService<IHostEnvironment>();

        public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();

        public TelegramBotApplication(IHost host)
        {
            _host = host;
            _pipelineBuilder = new PipelineBuilder(_host.Services);
        }

        public void Dispose() => _host.Dispose();

        public Task StartAsync(CancellationToken cancellationToken = default)
            => _host.StartAsync(cancellationToken);

        public Task StopAsync(CancellationToken cancellationToken = default)
            => _host.StopAsync(cancellationToken);

        IPipelineBuilder IPipelineBuilder.Use(Func<UpdateDelegate, UpdateDelegate> updateDelegate)
            => _pipelineBuilder.Use(updateDelegate);

        UpdateDelegate IPipelineBuilder.BuildPipeline()
            => _pipelineBuilder.BuildPipeline();
    }
}
