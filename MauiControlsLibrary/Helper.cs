using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

namespace MauiControlsLibrary
{
    /// <summary>
    /// Provides utility methods and classes for handling various operations related to Maui controls.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Encapsulates properties related to fonts, such as the font itself, font color, font size, and alignments.
        /// </summary>
        public class StandardFontPropterties(Microsoft.Maui.Graphics.Font? font = null, Color? fontColor = null, int? fontSize = null,
                HorizontalAlignment? horizontalAlignment = null, VerticalAlignment? verticalAlignment = null)
        {
            /// <summary>
            /// Gets or sets the font.
            /// </summary>
            public Microsoft.Maui.Graphics.Font Font { get; set; } = font ?? new Microsoft.Maui.Graphics.Font("Arial");

            /// <summary>
            /// Gets or sets the font color.
            /// </summary>
            public Color FontColor { get; set; } = fontColor ?? Colors.Black;

            /// <summary>
            /// Gets or sets the font size.
            /// </summary>
            public int FontSize { get; set; } = fontSize ?? 18;

            /// <summary>
            /// Gets or sets the horizontal alignment.
            /// </summary>
            public HorizontalAlignment HorizontalAlignment { get; set; } = horizontalAlignment ?? HorizontalAlignment.Center;

            /// <summary>
            /// Gets or sets the vertical alignment.
            /// </summary>
            public VerticalAlignment VerticalAlignment { get; set; } = verticalAlignment ?? VerticalAlignment.Center;
        }

        /// <summary>
        /// Sets the font attributes of a given ICanvas object using an instance of StandardFontPropterties.
        /// </summary>
        /// <param name="canvas">The ICanvas object to set the font attributes for.</param>
        /// <param name="standardFontPropterties">The StandardFontPropterties instance containing the font attributes to set.</param>
        public static void SetFontAttributes(ICanvas canvas, StandardFontPropterties standardFontPropterties)
        {
            canvas.Font = standardFontPropterties.Font;
            canvas.FontColor = standardFontPropterties.FontColor;
            canvas.FontSize = standardFontPropterties.FontSize;
        }

        /// <summary>
        /// Loads an image from the assembly's manifest resources using a given path. If the path is null or empty, it returns null.
        /// </summary>
        /// <param name="assembly">The assembly to load the image from.</param>
        /// <param name="manifestResourcePath">The path to the image in the assembly's manifest resources.</param>
        /// <returns>The loaded image, or null if the path is null or empty.</returns>
        public static Microsoft.Maui.Graphics.IImage? LoadImage(Assembly assembly, string? manifestResourcePath = null)
        {
            if (!string.IsNullOrEmpty(manifestResourcePath))
            {
                using Stream? stream = assembly.GetManifestResourceStream(manifestResourcePath);
                return PlatformImage.FromStream(stream);
            }
            return null;
        }

        /// <summary>
        /// Checks if a given PointF value is within a specified range.
        /// </summary>
        /// <param name="point">The PointF value to check.</param>
        /// <param name="xMinValue">The minimum X value of the range.</param>
        /// <param name="xMaxValue">The maximum X value of the range.</param>
        /// <param name="yMinValue">The minimum Y value of the range.</param>
        /// <param name="yMaxValue">The maximum Y value of the range.</param>
        /// <returns>True if the PointF value is within the range, false otherwise.</returns>
        public static bool PointFValueIsInRange(PointF? point, double xMinValue, double xMaxValue, double yMinValue, double yMaxValue)
        {
            return point.HasValue && point.Value.X >= xMinValue && point.Value.X <= xMaxValue && point.Value.Y >= yMinValue &&
                point.Value.Y <= yMaxValue;
        }

        /// <summary>
        /// Checks if a given integer value is within a specified range.
        /// </summary>
        /// <param name="valueToCheck">The integer value to check.</param>
        /// <param name="minValue">The minimum value of the range.</param>
        /// <param name="maxValue">The maximum value of the range.</param>
        /// <returns>True if the integer value is within the range, false otherwise.</returns>
        public static bool IntValueIsInRange(int valueToCheck, int minValue, int maxValue)
        {
            return valueToCheck >= minValue && valueToCheck <= maxValue;
        }

        /// <summary>
        /// Checks if a given integer value is less than 0 or greater than a maximum value, and resets it to a specified value if it is.
        /// </summary>
        /// <param name="value">The integer value to check and possibly reset.</param>
        /// <param name="minValue">The minimum value of the range.</param>
        /// <param name="maxValue">The maximum value of the range.</param>
        /// <param name="lessThanMinValueSet">The value to reset to if the integer value is less than 0. If null, the minimum value of the range is used.</param>
        /// <param name="moreThanMaxValueSet">The value to reset to if the integer value is greater than the maximum value of the range. If null, the maximum value of the range is used.</param>
        /// <returns>The possibly reset integer value.</returns>
        public static int ValueResetOnBoundsCheck(int value, int minValue, int maxValue, int? lessThanMinValueSet = null, int? moreThanMaxValueSet = null)
        {
            if (value < 0)
            {
                value = lessThanMinValueSet ?? minValue;
            }

            if (value > maxValue)
            {
                value = moreThanMaxValueSet ?? maxValue;
            }

            return value;
        }

        /// <summary>
        /// Checks if a given array is not null and not empty.
        /// </summary>
        /// <param name="array">The array to check.</param>
        /// <returns>True if the array is not null and not empty, false otherwise.</returns>
        public static bool ArrayNotNullOrEmpty(Array? array)
        {
            return array != null && array.Length > 0;
        }

        /// <summary>
        /// Creates a TapGestureRecognizer, attaches a provided event handler to its Tapped event, and adds it to a provided list of gesture recognizers.
        /// </summary>
        /// <param name="tapGestureRecognizer_Tapped">The event handler to attach to the Tapped event of the TapGestureRecognizer.</param>
        /// <param name="gestureRecognizers">The list of gesture recognizers to add the TapGestureRecognizer to.</param>
        public static void CreateTapGestureRecognizer(EventHandler<TappedEventArgs> tapGestureRecognizer_Tapped,
            IList<IGestureRecognizer> gestureRecognizers)
        {
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += tapGestureRecognizer_Tapped;
            gestureRecognizers.Add(tapGestureRecognizer);
        }

        /// <summary>
        /// Creates a PanGestureRecognizer, attaches a provided event handler to its PanUpdated event, and adds it to a provided list of gesture recognizers.
        /// </summary>
        /// <param name="panGesture_PanUpdated">The event handler to attach to the PanUpdated event of the PanGestureRecognizer.</param>
        /// <param name="gestureRecognizers">The list of gesture recognizers to add the PanGestureRecognizer to.</param>
        public static void CreatePanGestureRecognizer(EventHandler<PanUpdatedEventArgs> panGesture_PanUpdated,
            IList<IGestureRecognizer> gestureRecognizers)
        {
            PanGestureRecognizer panGesture = new();
            panGesture.PanUpdated += panGesture_PanUpdated;
            gestureRecognizers.Add(panGesture);
        }
    }
}
