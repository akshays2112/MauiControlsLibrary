using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImageProgressBar : MCLProgressBar
    {
        public new Color? ProgressBarColor { get; }
        public Microsoft.Maui.Graphics.IImage? ProgressBarBackgroundImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? ProgressBarImage { get; set; }
        public bool ClipProgressBarImage { get; set; } = true;

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (ProgressBarBackgroundColor != null)
            {
                canvas.FillColor = ProgressBarBackgroundColor;
                canvas.FillRoundedRectangle(new RectF(0, 0, (float)Width, (float)Height), RoundedRectangleRadius);
            }
            if (ProgressBarBackgroundImage != null)
                canvas.DrawImage(ProgressBarImage, 0, 0, (float)Width, (float)Height);
            int progressBarFillLengthInPixels = (int)(CurrentValue * (ArrangeHorizontal ? (decimal)Width : (decimal)Height) / (MaxValue - MinValue));
            if (CurrentValue > MinValue)
            {
                if (ProgressBarImage != null)
                {
                    canvas.SaveState();
                    canvas.ClipRectangle(new RectF(0, (float)(ArrangeHorizontal ? 0 : Height - progressBarFillLengthInPixels),
                        (float)(ArrangeHorizontal ? progressBarFillLengthInPixels : Width),
                        (float)(ArrangeHorizontal ? Height : progressBarFillLengthInPixels)));
                    if (ClipProgressBarImage)
                        canvas.DrawImage(ProgressBarImage, 0, 0, (float)Width, (float)Height);
                    else
                    {
                        canvas.DrawImage(ProgressBarImage, 0, (float)(ArrangeHorizontal ? 0 : Height - progressBarFillLengthInPixels),
                            (float)(ArrangeHorizontal ? progressBarFillLengthInPixels : Width),
                            (float)(ArrangeHorizontal ? Height : progressBarFillLengthInPixels));
                    }
                    canvas.ResetState();
                }
                else
                {
                    canvas.FillColor = ProgressBarColor;
                    canvas.FillRoundedRectangle(new RectF(0, (float)(ArrangeHorizontal ? 0 : Height - progressBarFillLengthInPixels),
                        (float)(ArrangeHorizontal ? progressBarFillLengthInPixels : Width),
                        (float)(ArrangeHorizontal ? Height : progressBarFillLengthInPixels)), RoundedRectangleRadius);
                }
            }
            Helper.SetFontAttributes(canvas, LabelFont);
            canvas.DrawString($"{BeforeValueLabel}{CurrentValue}{AfterValueLabel}", 0, 0, (float)Width, (float)Height,
                LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
        }

        public void LoadProgressBarImages(Assembly assembly, string? manifestResourcePathBackgroundImage, string? manifestResourcePathProgressBarImage)
        {
            ProgressBarBackgroundImage = Helper.LoadImage(assembly, manifestResourcePathBackgroundImage);
            ProgressBarImage = Helper.LoadImage(assembly, manifestResourcePathProgressBarImage);
        }
    }
}
