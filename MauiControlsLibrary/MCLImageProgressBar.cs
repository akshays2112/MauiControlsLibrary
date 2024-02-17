using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImageProgressBar : MCLProgressBarBase
    {
        public Microsoft.Maui.Graphics.IImage? ProgressBarBackgroundImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? ProgressBarImage { get; set; }
        public bool ClipProgressBarImage { get; set; } = true;

        public override void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            if (ProgressBarBackgroundImage != null)
            {
                canvas.DrawImage(ProgressBarImage, x, y, width, height);
            }
            else if (ProgressBarBackgroundColor != null)
            {
                canvas.FillColor = ProgressBarBackgroundColor;
                canvas.FillRoundedRectangle(new RectF(x, y, width, height), RoundedRectangleRadius);
            }
        }

        public override void DrawProgressBar(ICanvas canvas, float x, float y, float width, float height, int progressBarFillLengthInPixels)
        {
            if (ProgressBarImage != null)
            {
                canvas.SaveState();
                canvas.ClipRectangle(new RectF(x, y, width, height));
                if (ClipProgressBarImage)
                {
                    canvas.DrawImage(ProgressBarImage, 0, 0, (float)this.Width, (float)this.Height);
                }
                else
                {
                    canvas.DrawImage(ProgressBarImage, x, y, width, height);
                }
                canvas.ResetState();
            }
        }

        public void LoadProgressBarImages(Assembly assembly, string? manifestResourcePathBackgroundImage, string? manifestResourcePathProgressBarImage)
        {
            ProgressBarBackgroundImage = Helper.LoadImage(assembly, manifestResourcePathBackgroundImage);
            ProgressBarImage = Helper.LoadImage(assembly, manifestResourcePathProgressBarImage);
        }
    }
}
