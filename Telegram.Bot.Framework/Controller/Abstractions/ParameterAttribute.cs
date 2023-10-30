using System;
using System.Reflection;
using Telegram.Bot.Framework.Controller.Abstractions;
using Telegram.Bot.Framework.Filter;

namespace Telegram.Bot.Framework.Controller
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public abstract class ParameterAttribute : Attribute
    {
        public IUpdateFilter? Filter { get; }

        public abstract Func<ParameterInfo, IParameterFactory> Factory { get; }
    }
}