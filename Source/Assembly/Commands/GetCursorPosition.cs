using PoshCode.Pansies.ColorSpaces;
using System;
using System.Management.Automation;
using System.Management.Automation.Host;

namespace PoshCode.Pansies.Commands
{


    [Cmdlet("Get","CursorPosition")]
    public class GetCursorPositionCommand : Cmdlet
    {
        [Parameter()]
        [ValidateRange(50, int.MaxValue)]
        public int WaitMilliseconds { get; set; } = 500;

        protected override void EndProcessing()
        {
            Console.Write("\e[?6n");
            // The terminal should respond with "input" but it's not instantaneous
            while(!Console.KeyAvailable && WaitMilliseconds > 0)
            {
                WaitMilliseconds -= 10;
                System.Threading.Thread.Sleep(10);
            }
            string response = string.Empty;
            while (Console.KeyAvailable)
            {
                response += Console.ReadKey(true).KeyChar;
            }
            if (string.IsNullOrEmpty(response)) {
                WriteWarning("No response received from terminal. Ensure this terminal supports cursor position queries.");
                return;
            }
            if (!response.StartsWith("\e[?") || !response.EndsWith("R"))
            {
                WriteError(new ErrorRecord(new InvalidOperationException($"Unexpected response from terminal. Expected: \"ESC [ <r> ; <c> R\" Where <r> = cursor row and <c> = cursor column. But got \"{response.Replace("\e", "`e")}\""), "InvalidResponse", ErrorCategory.InvalidResult, null));
                return;
            }

            WriteVerbose(response.Replace("\e", "`e"));
            var match = System.Text.RegularExpressions.Regex.Match(response, @"\e\[\??((?:\d+;)?\d+;\d+)R");
            var parts = match.Groups[1].Value.Split(';');
            if (parts.Length < 2)
            {
                WriteError(new ErrorRecord(new InvalidOperationException($"Unexpected response from terminal. Expected: \"ESC [ <r> ; <c> R\" Where <r> = cursor row and <c> = cursor column. But got \"{response.Replace("\e", "`e")}\""), "InvalidResponse", ErrorCategory.InvalidResult, null));
                return;
            }
            int row = int.Parse(parts[0]);
            int col = int.Parse(parts[1]);
            int page = parts.Length > 2 ? int.Parse(parts[2]) : 1;

            var result = new Position(row, col, true);
            WriteObject(result);

        }
    }
}
