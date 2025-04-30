using PoshCode.Pansies.ColorSpaces.Comparisons;
using PoshCode.Pansies.Palettes;

namespace PoshCode.Pansies.Palettes
{
    public class TerminalPalette : NamedPalette<RgbColor>
    {
        public override IColorSpaceComparison ComparisonAlgorithm { get; set; } = new CieDe2000Comparison();

        public TerminalPalette()
        {
            Add("Black", new RgbColor(System.ConsoleColor.Black));
            Add("Red", new RgbColor(System.ConsoleColor.DarkRed));
            Add("Green", new RgbColor(System.ConsoleColor.DarkGreen));
            Add("Yellow", new RgbColor(System.ConsoleColor.DarkYellow));
            Add("Blue", new RgbColor(System.ConsoleColor.DarkBlue));
            Add("Magenta", new RgbColor(System.ConsoleColor.DarkMagenta));
            Add("Cyan", new RgbColor(System.ConsoleColor.DarkCyan));
            Add("White", new RgbColor(System.ConsoleColor.Gray));
            Add("BrightBlack", new RgbColor(System.ConsoleColor.DarkGray));
            Add("BrightRed", new RgbColor(System.ConsoleColor.Red));
            Add("BrightGreen", new RgbColor(System.ConsoleColor.Green));
            Add("BrightYellow", new RgbColor(System.ConsoleColor.Yellow));
            Add("BrightBlue", new RgbColor(System.ConsoleColor.Blue));
            Add("BrightMagenta", new RgbColor(System.ConsoleColor.Magenta));
            Add("BrightCyan", new RgbColor(System.ConsoleColor.Cyan));
            Add("BrightWhite", new RgbColor(System.ConsoleColor.White));
            Add("Default", new RgbColor("Default"));
        }
    }
}
