using PoshCode.Pansies.ColorSpaces;
using System;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace PoshCode.Pansies.Commands
{


    [Cmdlet("Set","CursorPosition")]
    public class SetCursorPositionCommand : Cmdlet
    {
        [Parameter(Position = 1)]
        public int? Column { get; set; }

        [Parameter(Position = 2)]
        public int? Line { get; set; }

        [Parameter()]
        public SwitchParameter Absolute { get; set; } = false;

        protected override void EndProcessing()
        {
            var result = new Position(Line, Column, Absolute);
            WriteObject(result.ToString());
        }
    }
}
