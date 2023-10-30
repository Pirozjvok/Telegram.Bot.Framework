using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Framework.Updates;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Filter
{
    public class UpdateTypeFilter : IUpdateFilter
    {
        private HashSet<UpdateType> _updateTypes;

        public UpdateTypeFilter(params UpdateType[] updateTypes)
        {
            _updateTypes = new HashSet<UpdateType>(updateTypes);
        }

        public bool Check(IUpdateContext update)
        {
            return _updateTypes.Contains(update.Update.Type);
        }
    }
}
