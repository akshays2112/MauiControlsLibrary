using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
