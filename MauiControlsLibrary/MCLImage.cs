using System.Reflection;

namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents an image in the Maui Controls Library.
    /// </summary>
    public class MCLImage : GraphicsView, IDrawable
    {
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? Image { get; set; }

        /// <summary>
        /// Gets or sets the title of the image.
        /// </summary>
        public string ImageTitle { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the tap areas of the image.
        /// </summary>
        public RectF[]? ImageTapAreas { get; set; }

        /// <summary>
        /// Occurs when the image is tapped.
        /// </summary>
        public event EventHandler<MCLImageEventArgs>? OnMCLImageTapped;

        public class MCLImageEventArgs(EventArgs eventArgs, int[] tappedImageAreasIndexes) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public int[]? TappedImageAreasIndexes { get; set; } = tappedImageAreasIndexes;
        }

        public MCLImage()
        {
            Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        /// <summary>
        /// Handles the Tapped event of the TapGestureRecognizer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The TappedEventArgs instance containing the event data.</param>
        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            ImageTapped(0, (float)Width, 0, (float)Height, ImageTapAreas, OnMCLImageTapped, Invalidate, this, e);
        }

        public static void ImageTapped(float x, float width, float y, float height, RectF[]? imageTapAreas,
            EventHandler<MCLImageEventArgs>? onMCLImageTapped, Action invalidate, GraphicsView sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(sender);

            if (Helper.PointFValueIsInRange(point, x, width, y, height))

            {

                List<int> tappedImageAreasIndexes = [];

                for (int i = 0; imageTapAreas != null && i < imageTapAreas.Length; i++)

                {

                    if (Helper.PointFValueIsInRange(point, imageTapAreas[i].X, imageTapAreas[i].X + imageTapAreas[i].Width,

                        imageTapAreas[i].Y, imageTapAreas[i].Y + imageTapAreas[i].Height))

                    {

                        tappedImageAreasIndexes.Add(i);

                    }

                }

                onMCLImageTapped?.Invoke(sender, new MCLImageEventArgs(e, tappedImageAreasIndexes.ToArray<int>()));

                invalidate();

            }
        }

        /// <summary>
        /// Draws the image on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area of the canvas that needs to be redrawn.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Image != null)
                DrawImage(canvas, 0, 0, Image.Width, Image.Height);
        }

        /// <summary>
        /// Draws the image on the canvas at the specified location and size.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the image.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the image.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        public virtual void DrawImage(ICanvas canvas, float x, float y, float width, float height)
        {
            if (Image != null)
                canvas.DrawImage(Image, x, y, width, height);
        }

        /// <summary>
        /// Loads the image from the specified assembly and manifest resource path.
        /// </summary>
        /// <param name="assembly">The assembly that contains the image.</param>
        /// <param name="manifestResourcePath">The manifest resource path of the image.</param>
        public virtual void LoadImage(Assembly assembly, string manifestResourcePath)
        {
            Image = Helper.LoadImage(assembly, manifestResourcePath);
            if (Image != null)
            {
                WidthRequest = Image.Width;
                HeightRequest = Image.Height;
            }
        }
    }
}
