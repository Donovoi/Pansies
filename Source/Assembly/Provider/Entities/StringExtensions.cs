using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PoshCode.Pansies
{
    public static class StringExtensions
    {

        public static IEnumerable<int> ToUTF32(this string value)
        {
            for (var i = 0; i < value.Length; i++)
            {
                if (char.IsHighSurrogate(value[i]))
                {
                    if (value.Length < (i + 2) || !char.IsLowSurrogate(value[i + 1]))
                    {
                        throw new InvalidDataException();
                    }
                    yield return char.ConvertToUtf32(value[i], value[++i]);
                }
                else
                {
                    yield return value[i];
                }
            }
        }
        public static string ToPsEscapedString(this string value)
        {
            var result = new StringBuilder();

            for (var i = 0; i < value.Length; i++)
            {
                if (char.IsHighSurrogate(value[i]))
                {
                    if (value.Length < (i + 2) || !char.IsLowSurrogate(value[i + 1]))
                    {
                        throw new InvalidDataException();
                    }
                    result.AppendFormat("`u{{{0:x6}}}", char.ConvertToUtf32(value[i], value[++i]));
                }
                else
                {
                    if (value[i] == '\e')
                    {
                        result.Append("`e");
                    }
                    else if (value[i] == '`')
                    {
                        result.Append("``");
                    }
                    else if (value[i] == '$')
                    {
                        result.Append("`$");
                    }
                    else if (value[i] == '"')
                    {
                        result.Append("`\"");
                    }
                    else if (((int)value[i]) < 31 || ((int)value[i]) > 126)
                    {
                        result.AppendFormat("`u{{{0:x4}}}", (int)value[i]);
                    }
                    else
                    {
                        result.Append(value[i]);
                    }
                }
            }
            return result.ToString();
        }
    }
}
