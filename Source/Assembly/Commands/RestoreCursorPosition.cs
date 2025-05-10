using PoshCode.Pansies.ColorSpaces;
using System;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace PoshCode.Pansies.Commands
{


    [Cmdlet("Restore","CursorPosition")]
    public class RestoreCursorPositionCommand : Cmdlet
    {
        protected override void EndProcessing()
        {
            Console.Write("\e8");
        }
    }
}
