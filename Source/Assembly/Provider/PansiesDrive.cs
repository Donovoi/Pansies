using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using CodeOwls.PowerShell.Paths.Processors;
using CodeOwls.PowerShell.Provider.PathNodes;

namespace PoshCode.Pansies.Provider
{
    public class PansiesDrive : CodeOwls.PowerShell.Provider.Drive
    {
        // PSDriveRoot constructor
        public PansiesDrive(PSDriveInfo name): base(name)
        {

        }
        public PansiesDrive(PSDriveInfo info, IPathResolver pathResolver) : base(info, pathResolver)
        {

        }
    }
}
