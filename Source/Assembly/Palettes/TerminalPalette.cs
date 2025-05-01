using PoshCode.Pansies.ColorSpaces.Comparisons;
using PoshCode.Pansies.Palettes;

namespace PoshCode.Pansies.Palettes
{
    public class TerminalPalette : NamedPalette<RgbColor>
    {
        public override IColorSpaceComparison ComparisonAlgorithm { get; set; } = new CieDe2000Comparison();

        public TerminalPalette()
        {
            Add("Black", new RgbColor(0x00, 0x00, 0x00, ColorMode.TerminalColor));
            Add("Red", new RgbColor(0x80, 0x00, 0x00, ColorMode.TerminalColor));
            Add("Green", new RgbColor(0x00, 0x80, 0x00, ColorMode.TerminalColor));
            Add("Yellow", new RgbColor(0x80, 0x80, 0x00, ColorMode.TerminalColor));
            Add("Blue", new RgbColor(0x00, 0x00, 0x80, ColorMode.TerminalColor));
            Add("Magenta", new RgbColor(0x80, 0x00, 0x80, ColorMode.TerminalColor));
            Add("Cyan", new RgbColor(0x00, 0x80, 0x80, ColorMode.TerminalColor));
            Add("White", new RgbColor(0xc0, 0xc0, 0xc0, ColorMode.TerminalColor));
            Add("BrightBlack", new RgbColor(0x80, 0x80, 0x80, ColorMode.TerminalColor));
            Add("BrightRed", new RgbColor(0xff, 0x00, 0x00, ColorMode.TerminalColor));
            Add("BrightGreen", new RgbColor(0x00, 0xff, 0x00, ColorMode.TerminalColor));
            Add("BrightYellow", new RgbColor(0xff, 0xff, 0x00, ColorMode.TerminalColor));
            Add("BrightBlue", new RgbColor(0x00, 0x00, 0xff, ColorMode.TerminalColor));
            Add("BrightMagenta", new RgbColor(0xff, 0x00, 0xff, ColorMode.TerminalColor));
            Add("BrightCyan", new RgbColor(0x00, 0xff, 0xff, ColorMode.TerminalColor));
            Add("BrightWhite", new RgbColor(0xff, 0xff, 0xff, ColorMode.TerminalColor));
            Add("Default", new RgbColor(-1,-1,-1, ColorMode.TerminalColor));
        }
    }
}
