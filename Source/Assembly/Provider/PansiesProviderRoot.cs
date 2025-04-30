using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeOwls.PowerShell.Provider.PathNodes;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using PoshCode.Pansies.Palettes;

namespace PoshCode.Pansies.Provider
{
    class PansiesProviderRoot : PathNodeBase
    {
        public override IPathValue GetNodeValue()
        {
            return new ContainerPathValue(this, Name);
        }

        public override string Name
        {
            get { return "Pansies"; }
        }
    }
}
