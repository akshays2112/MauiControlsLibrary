using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

namespace MauiControlsLibrary
{
    public static class Helper
    {
        public static void SetFontAttributes(ICanvas canvas, Microsoft.Maui.Graphics.Font font, Color fontColor, int fontSize)
        {
            canvas.Font = font;
            canvas.FontColor = fontColor;
            canvas.FontSize = fontSize;
        }

        public static Microsoft.Maui.Graphics.IImage? LoadImage(Assembly assembly, string manifestResourcePath)
        {
            using (Stream? stream = assembly.GetManifestResourceStream(manifestResourcePath))
            {
                return PlatformImage.FromStream(stream);
            }
        }
    }
}
