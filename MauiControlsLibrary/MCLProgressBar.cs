namespace MauiControlsLibrary
{
    public class MCLProgressBar : MCLProgressBarBase
    {
        public Color ProgressBarColor { get; set; } = Colors.Green;

        public override void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.FillColor = ProgressBarBackgroundColor;
            canvas.FillRoundedRectangle(new RectF(0, 0, (float)Width, (float)Height), RoundedRectangleRadius);
        }

        public override void DrawProgressBar(ICanvas canvas, float x, float y, float width, float height, int progressBarFillLengthInPixels) 
        {
            canvas.FillColor = ProgressBarColor;
            canvas.FillRoundedRectangle(new RectF(x, y, width, height), RoundedRectangleRadius);
        }
    }
}
