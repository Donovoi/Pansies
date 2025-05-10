using PoshCode.Pansies.ColorSpaces;
using System;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace PoshCode.Pansies.Commands
{


    [Cmdlet("Save","CursorPosition")]
    public class SaveCursorPositionCommand : Cmdlet
    {
        protected override void EndProcessing()
        {
            Console.Write("\e[s");
        }
    }
}
