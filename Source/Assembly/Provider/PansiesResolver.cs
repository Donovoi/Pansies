using CodeOwls.PowerShell.Paths.Processors;
using CodeOwls.PowerShell.Provider;
using CodeOwls.PowerShell.Provider.PathNodeProcessors;
using CodeOwls.PowerShell.Provider.PathNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PoshCode.Pansies.Provider
{
    class PansiesResolver(Func<IPathNode> RootFactory) : PathResolverBase
    {
        private readonly Func<IPathNode> rootFactory = RootFactory;

        /// <summary>
        /// returns the first node factory object in the path graph
        /// </summary>
        protected override IPathNode Root
        {
            get
            {
                return rootFactory.Invoke();
            }
        }
    }
}
