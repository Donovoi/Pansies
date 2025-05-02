using PoshCode.Pansies.ColorSpaces.Comparisons;
using System.Linq;

namespace PoshCode.Pansies.ColorSpaces
{
    public delegate double ComparisonAlgorithm(IColorSpace a, IColorSpace b);

    /// <summary>
    /// Defines the public methods for all color spaces
    /// </summary>
    public interface IColorSpace
    {
        /// <summary>
        /// Initialize settings from an Rgb object
        /// </summary>
        /// <param name="color"></param>
        void Initialize(IRgb color);

        /// <summary>
        /// Convert the color space to Rgb, you should probably using the "To" method instead. Need to figure out a way to "hide" or otherwise remove this method from the public interface.
        /// </summary>
        /// <returns></returns>
        IRgb ToRgb();

        /// <summary>
        /// Convert any IColorSpace to any other IColorSpace.
        /// </summary>
        /// <typeparam name="T">IColorSpace type to convert to</typeparam>
        /// <returns></returns>
        T To<T>() where T : IColorSpace, new();

        /// <summary>
        /// Determine how close two IColorSpaces are to each other using a passed in algorithm
        /// </summary>
        /// <param name="compareToValue">Other IColorSpace to compare to</param>
        /// <param name="comparer">Algorithm to use for comparison</param>
        /// <returns>Distance in 3d space as double</returns>
        double Compare(IColorSpace compareToValue, IColorSpaceComparison comparer);

        /// <summary>
        /// Array of signifigant values in a consistent order. Useful for generic n-dimensional math.
        /// </summary>
        double[] Ordinals { get; set; }

        /// <summary>
        /// Convert the color space to a virtual terminal RGB escape sequence
        /// </summary>
        /// <param name="background">If true, use ESC 48, otherwise use ESC 38</param>
        /// <returns>ESC [ 48 ; 2 ; R ; G ; B m</returns>
        string ToVTEscapeSequence(bool background = false);
    }

    /// <summary>
    /// Abstract ColorSpace class, defines the To method that converts between any IColorSpace.
    /// </summary>
    public abstract class ColorSpace : IColorSpace
    {
        public abstract void Initialize(IRgb color);
        public abstract IRgb ToRgb();
        public abstract double[] Ordinals { get; set; }
        internal abstract string[] OrdinalLabels { get; }

        public override string ToString()
        {
            string[] fields = new string[Ordinals.Length];

            for (int i = 0; i < Ordinals.Length; i++)
            {
                // if any of the ordinals are negative, return "Terminal Default"
                if (Ordinals[i] < 0)
                {
                    return "Terminal Default";
                }
                string value;
                if (Ordinals[i] >= 0 && Ordinals[i] <= 1) {
                    value = Ordinals[i].ToString("N3");
                } else {
                    value = Ordinals[i].ToString("N0");
                }
                fields[i] = $"{OrdinalLabels[i]}={value}";
            }

            return string.Join("; ", fields);
        }

        /// <summary>
        /// Convienience method for comparing any IColorSpace
        /// </summary>
        /// <param name="compareToValue"></param>
        /// <param name="comparer"></param>
        /// <returns>Single number representing the difference between two colors</returns>
        public double Compare(IColorSpace compareToValue, IColorSpaceComparison comparer)
        {
            return comparer.Compare(this, compareToValue);
        }

        /// <summary>
        /// Convert any IColorSpace to any other IColorSpace
        /// </summary>
        /// <typeparam name="T">Must implement IColorSpace, new()</typeparam>
        /// <returns></returns>
        public T To<T>() where T : IColorSpace, new()
        {
            if (typeof(T) == GetType())
            {
                return (T)MemberwiseClone();
            }

            var newColorSpace = new T();
            newColorSpace.Initialize(ToRgb());

            return newColorSpace;
        }


        public T[] GradientTo<T>(T end, int size = 10) where T : IColorSpace, new()
        {
            T start = new T();
            start.Initialize(ToRgb());
            return Gradient.GetGradient(start, end, size).ToArray();
        }

        /// <summary>
        /// Convert the color space to a virtual terminal RGB foreground color escape sequence
        /// </summary>
        public string Fg => ToVTEscapeSequence(false);

        /// <summary>
        /// Convert the color space to a virtual terminal RGB background color escape sequence
        /// </summary>
        public string Bg => ToVTEscapeSequence(true);

        /// <summary>
        /// Convert the color space to a virtual terminal RGB escape sequence
        /// </summary>
        /// <param name="background">If true, use ESC 48, otherwise use ESC 38</param>
        /// <returns>ESC [ 48 ; 2 ; R ; G ; B m</returns>
        public virtual string ToVTEscapeSequence(bool background = false)
        {
            var rgb = ToRgb();
            return string.Format(background ? "\u001B[48;2;{0:n0};{1:n0};{2:n0}m" : "\u001B[38;2;{0:n0};{1:n0};{2:n0}m", rgb.R, rgb.G, rgb.B);
        }
    }
}
