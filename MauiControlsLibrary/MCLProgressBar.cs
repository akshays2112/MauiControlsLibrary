namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a progress bar in the Maui Controls Library.
    /// </summary>
    public class MCLProgressBar : MCLProgressBarBase
    {
        /// <summary>
        /// Gets or sets the color of the progress bar.
        /// </summary>
        public Color ProgressBarColor { get; set; } = Colors.Green;

        /// <summary>
        /// Draws the frame of the progress bar on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="width">The width of the progress bar.</param>
        /// <param name="height">The height of the progress bar.</param>
        public override void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.FillColor = ProgressBarBackgroundColor;
            canvas.FillRoundedRectangle(new RectF(0, 0, (float)Width, (float)Height), RoundedRectangleRadius);
        }

        /// <summary>
        /// Draws the progress bar on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="width">The width of the progress bar.</param>
        /// <param name="height">The height of the progress bar.</param>
        /// <param name="progressBarFillLengthInPixels">The fill length of the progress bar in pixels.</param>
        public override void DrawProgressBar(ICanvas canvas, float x, float y, float width, float height, int progressBarFillLengthInPixels)
        {
            canvas.FillColor = ProgressBarColor;
            canvas.FillRoundedRectangle(new RectF(x, y, width, height), RoundedRectangleRadius);
        }
    }
}
