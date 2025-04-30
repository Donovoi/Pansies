using System.Linq;
using System.Management.Automation;

namespace PoshCode.Pansies.Provider
{
    public class NerdFontDrive : CodeOwls.PowerShell.Provider.Drive
    {
        public NerdFontDrive(PSDriveInfo name): base(name)
        {

        }
    }
}
