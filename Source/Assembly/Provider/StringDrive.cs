using System.Linq;
using System.Management.Automation;

namespace PoshCode.Pansies.Provider
{
    public class StringDrive : CodeOwls.PowerShell.Provider.Drive
    {
        // PSDriveRoot constructor
        public StringDrive(PSDriveInfo name): base(name)
        {

        }
    }
}
