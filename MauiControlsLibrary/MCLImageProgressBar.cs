using System.Reflection;

namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a progress bar with images in the Maui Controls Library.
    /// </summary>
    public class MCLImageProgressBar : MCLProgressBarBase
    {
        /// <summary>
        /// Gets or sets the background image of the progress bar.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? ProgressBarBackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets the image of the progress bar.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? ProgressBarImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the progress bar image is clipped.
        /// </summary>
        public bool ClipProgressBarImage { get; set; } = true;

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

        /// <summary>
        /// Loads the images of the progress bar from the specified assembly and manifest resource paths.
        /// </summary>
        /// <param name="assembly">The assembly that contains the images.</param>
        /// <param name="manifestResourcePathBackgroundImage">The manifest resource path of the background image.</param>
        /// <param name="manifestResourcePathProgressBarImage">The manifest resource path of the progress bar image.</param>
        public void LoadProgressBarImages(Assembly assembly, string? manifestResourcePathBackgroundImage, string? manifestResourcePathProgressBarImage)
        {
            ProgressBarBackgroundImage = Helper.LoadImage(assembly, manifestResourcePathBackgroundImage);
            ProgressBarImage = Helper.LoadImage(assembly, manifestResourcePathProgressBarImage);
        }
    }
}
