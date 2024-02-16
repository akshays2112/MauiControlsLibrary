using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

namespace MauiControlsLibrary
{
    public static class Helper
    {
        public class StandardFontPropterties(Microsoft.Maui.Graphics.Font? font = null, Color? fontColor = null, int? fontSize = null,
                HorizontalAlignment? horizontalAlignment = null, VerticalAlignment? verticalAlignment = null)
        {
            public Microsoft.Maui.Graphics.Font Font { get; set; } = font ?? new Microsoft.Maui.Graphics.Font("Arial");
            public Color FontColor { get; set; } = fontColor ?? Colors.Black;
            public int FontSize { get; set; } = fontSize ?? 18;
            public HorizontalAlignment HorizontalAlignment { get; set; } = horizontalAlignment ?? HorizontalAlignment.Center;
            public VerticalAlignment VerticalAlignment { get; set; } = verticalAlignment ?? VerticalAlignment.Center;
        }

        public static void SetFontAttributes(ICanvas canvas, StandardFontPropterties standardFontPropterties)
        {
            canvas.Font = standardFontPropterties.Font;
            canvas.FontColor = standardFontPropterties.FontColor;
            canvas.FontSize = standardFontPropterties.FontSize;
        }

        public static Microsoft.Maui.Graphics.IImage? LoadImage(Assembly assembly, string? manifestResourcePath = null)
        {
            if (!string.IsNullOrEmpty(manifestResourcePath))
            {
                using Stream? stream = assembly.GetManifestResourceStream(manifestResourcePath);
                return PlatformImage.FromStream(stream);
            }
            return null;
        }

        public static bool PointFValueIsInRange(PointF? point, double xMinValue, double xMaxValue, double yMinValue, double yMaxValue)
        {
            return point.HasValue && point.Value.X >= xMinValue && point.Value.X <= xMaxValue && point.Value.Y >= yMinValue &&
                point.Value.Y <= yMaxValue;
        }

        public static bool IntValueIsInRange(int valueToCheck, int minValue, int maxValue)
        {
            return valueToCheck >= minValue && valueToCheck <= maxValue;
        }

        public static int ValueResetOnBoundsCheck(int value, int minValue, int maxValue, int? lessThanMinValueSet = null, int? moreThanMaxValueSet = null)
        {
            if (value < 0)
                value = lessThanMinValueSet ?? minValue;
            if (value > maxValue)
                value = moreThanMaxValueSet ?? maxValue;
            return value;
        }

        public static bool ArrayNotNullOrEmpty(Array? array)
        {
            return array != null && array.Length > 0;
        }
    }
}
