using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImage : GraphicsView, IDrawable
    {
        public Microsoft.Maui.Graphics.IImage? Image { get; set; }
        public string ImageTitle { get; set; } = string.Empty;
        public RectF[]? ImageTapAreas { get; set; }
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

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Image != null)
                DrawImage(canvas, 0, 0, Image.Width, Image.Height);
        }

        public virtual void DrawImage(ICanvas canvas, float x, float y, float width, float height)
        {
            if (Image != null)
                canvas.DrawImage(Image, x, y, width, height);
        }

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
