using PoshCode.Pansies.ColorSpaces.Comparisons;
using PoshCode.Pansies.Palettes;

namespace PoshCode.Pansies.Palettes
{
    public class ConsolePalette : Palette<RgbColor>
    {
        public override IColorSpaceComparison ComparisonAlgorithm { get; set; } = new CieDe2000Comparison();

        public ConsolePalette()
        {
            // The ConsolePalette needs to be in the .NET ConsoleColors enum order
            Add(new RgbColor(0x00, 0x00, 0x00, ColorMode.ConsoleColor));
            Add(new RgbColor(0x00, 0x00, 0x80, ColorMode.ConsoleColor));
            Add(new RgbColor(0x00, 0x80, 0x00, ColorMode.ConsoleColor));
            Add(new RgbColor(0x00, 0x80, 0x80, ColorMode.ConsoleColor));
            Add(new RgbColor(0x80, 0x00, 0x00, ColorMode.ConsoleColor));
            Add(new RgbColor(0x80, 0x00, 0x80, ColorMode.ConsoleColor));
            Add(new RgbColor(0x80, 0x80, 0x00, ColorMode.ConsoleColor));
            Add(new RgbColor(0xc0, 0xc0, 0xc0, ColorMode.ConsoleColor));
            Add(new RgbColor(0x80, 0x80, 0x80, ColorMode.ConsoleColor));
            Add(new RgbColor(0x00, 0x00, 0xff, ColorMode.ConsoleColor));
            Add(new RgbColor(0x00, 0xff, 0x00, ColorMode.ConsoleColor));
            Add(new RgbColor(0x00, 0xff, 0xff, ColorMode.ConsoleColor));
            Add(new RgbColor(0xff, 0x00, 0x00, ColorMode.ConsoleColor));
            Add(new RgbColor(0xff, 0x00, 0xff, ColorMode.ConsoleColor));
            Add(new RgbColor(0xff, 0xff, 0x00, ColorMode.ConsoleColor));
            Add(new RgbColor(0xff, 0xff, 0xff, ColorMode.ConsoleColor));
        }
    }
}
