using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Telegram.Bot.Framework.Controller.Abstractions;

namespace Telegram.Bot.Framework.Controller
{
    public class FromStorageAttribute : ParameterAttribute
    {
        public override Func<ParameterInfo, IParameterFactory> Factory { get; } = info => new FromStorageFactory(info);
    }
}
