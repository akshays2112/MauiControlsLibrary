using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

namespace MauiControlsLibrary
{
    public static class Helper
    {
        public class StandardFontPropterties
        {
            public Microsoft.Maui.Graphics.Font Font { get; set; }
            public Color FontColor { get; set; }
            public int FontSize { get; set; }
            public HorizontalAlignment HorizontalAlignment { get; set; }
            public VerticalAlignment VerticalAlignment { get; set; }

            public StandardFontPropterties(Microsoft.Maui.Graphics.Font? font = null, Color? fontColor = null, int? fontSize = null, 
                HorizontalAlignment? horizontalAlignment = null, VerticalAlignment? verticalAlignment = null)
            {
                Font = font ?? new Microsoft.Maui.Graphics.Font("Arial");
                FontColor = fontColor ?? Colors.Black;
                FontSize = fontSize ?? 18;
                HorizontalAlignment = horizontalAlignment ?? HorizontalAlignment.Center;
                VerticalAlignment = verticalAlignment ?? VerticalAlignment.Center;
            }
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
                using (Stream? stream = assembly.GetManifestResourceStream(manifestResourcePath))
                {
                    return PlatformImage.FromStream(stream);
                }
            }
            return null;
        }

        public static bool PointFValueIsInRange(PointF? point, double xMinValue, double xMaxValue, double yMinValue, double yMaxValue) 
        {
            if (point.HasValue && point.Value.X >= xMinValue && point.Value.X <= xMaxValue && point.Value.Y >= yMinValue && 
                point.Value.Y <= yMaxValue)
                return true;
            return false;
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
    }
}
