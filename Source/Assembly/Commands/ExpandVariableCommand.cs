using PoshCode.Pansies.ColorSpaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Language;
using System.Text;

namespace PoshCode.Pansies.Commands
{


    [Cmdlet("Expand","Variable")]
    public class ExpandVariableCommand : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Path { get; set; }

        [Parameter()]
        public SwitchParameter Unescaped { get; set; }

        [Parameter()]
        public string[] Drive { get; set; } = ["bg", "emoji", "esc", "extra", "fg", "nf"];

        [Parameter()]
        public SwitchParameter InPlace { get; set; }

        protected override void EndProcessing()
        {
            var resolvedProviderPath = GetResolvedProviderPathFromPSPath(Path, out ProviderInfo provider);
            foreach (var file in resolvedProviderPath) {
                var result = new StringBuilder();
                var replacements = new List<TextReplacement>();
                Ast ast = null;
                string code;
                string fullName = file;
                if (provider.Name != "Variable" && provider.Name != "FileSystem" && !fullName.Contains(":"))
                {
                    fullName = $"{provider.Name}:{file}";
                }
                code = GetVariableValue(fullName).ToString();
                ast = Parser.ParseInput(code, fullName, out var tokens, out var parseErrors);
                if (parseErrors.Length > 0) {
                    WriteError(new ErrorRecord(new Exception($"{parseErrors.Length} Parse Errors in {fullName}, cannot expand."), "ParseErrors", ErrorCategory.InvalidOperation, fullName));
                    continue;
                }
                result.AppendLine(code);

                var variables = ast.FindAll(a => a is VariableExpressionAst, true).
                                    Cast<VariableExpressionAst>().
                                    Where(v => (!v.VariablePath.IsUnqualified ||    // variable:foo is IsUnqualified = False and IsVariable = True
                                                v.VariablePath.IsDriveQualified) && // fg:red is IsDriveQualified = True and IsVariable = False
                                                Drive.Contains(v.VariablePath.DriveName ?? "variable", StringComparer.OrdinalIgnoreCase));

                foreach (var variable in variables)
                {
                    try
                    {
                        var replacement = GetVariableValue(variable.VariablePath.UserPath).ToString();
                        if (!Unescaped)
                        {
                            replacement = replacement.ToPsEscapedString();
                        }
                        if (variable.Parent is ExpandableStringExpressionAst)
                        {
                            replacements.Add(new TextReplacement(replacement, variable.Extent));
                        }
                        else
                        {
                            replacements.Add(new TextReplacement('"' + replacement + '"', variable.Extent));
                        }
                    }
                    catch
                    {
                        WriteWarning($"VariableNotFound: '{variable.VariablePath.UserPath}' at {fullName}:{variable.Extent.StartLineNumber}:{variable.Extent.StartColumnNumber}");
                        continue;
                    }
                }

                foreach (var replacement in replacements.OrderByDescending(r => r.StartOffset))
                {
                    result.Remove(replacement.StartOffset, replacement.Length).Insert(replacement.StartOffset, replacement.Text);
                }

                if (InPlace)
                {
                    SessionState.PSVariable.Set(fullName, result.ToString());
                }
                else
                {
                    WriteObject(result.ToString());
                }

            }

        }

        private class TextReplacement(string text, IScriptExtent extent)
        {
            public int StartOffset = extent.StartOffset;
            public int EndOffset = extent.EndOffset;
            public string Text = text;
            public int Length => EndOffset - StartOffset;
        }

    }
}
