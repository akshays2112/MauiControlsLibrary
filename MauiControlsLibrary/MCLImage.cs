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
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Helper.PointFValueIsInRange(point, 0, Width, 0, Height))
                {
                    List<int> tappedImageAreasIndexes = [];
                    for (int i = 0; ImageTapAreas != null && i < ImageTapAreas.Length; i++)
                    {
                        if (Helper.PointFValueIsInRange(point, ImageTapAreas[i].X, ImageTapAreas[i].X + ImageTapAreas[i].Width,
                            ImageTapAreas[i].Y, ImageTapAreas[i].Y + ImageTapAreas[i].Height))
                            tappedImageAreasIndexes.Add(i);
                    }
                    OnMCLImageTapped?.Invoke(this, new MCLImageEventArgs(e, tappedImageAreasIndexes.ToArray<int>()));
                    Invalidate();
                }
            };
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Image != null)
                canvas.DrawImage(Image, 0, 0, Image.Width, Image.Height);
        }

        public void LoadImage(Assembly assembly, string manifestResourcePath)
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
