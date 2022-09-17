using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DealerPortal.Areas.HelpPage.ModelDescriptions
{
    public class EnumTypeModelDescription : ModelDescription
    {
        public EnumTypeModelDescription()
        {
            Orders = new Collection<EnumValueDescription>();
        }

        public Collection<EnumValueDescription> Orders { get; private set; }
    }
}