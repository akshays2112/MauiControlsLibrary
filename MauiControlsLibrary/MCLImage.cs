using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImage : GraphicsView, IDrawable
    {
        public Microsoft.Maui.Graphics.IImage? Image { get; set; }
        public string ImageTitle { get; set; } = string.Empty;
        public RectF[]? ImageTapAreas { get; set; }
        public event EventHandler<MCLImageEventArgs>? OnMCLImageTapped;

        public class MCLImageEventArgs : EventArgs
        {
            public EventArgs? EventArgs { get; set; }
            public int[]? TappedImageAreasIndexes { get; set; }

            public MCLImageEventArgs(EventArgs eventArgs, int[] tappedImageAreasIndexes)
            {
                EventArgs = eventArgs;
                TappedImageAreasIndexes = tappedImageAreasIndexes;
            }
        }

        public MCLImage()
        {
            this.Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Helper.PointFValueIsInRange(point, 0, this.Width, 0, this.Height))
                {

                    List<int> tappedImageAreasIndexes = new();
                    for (int i = 0; ImageTapAreas != null && i < ImageTapAreas.Length; i++)
                    {
                        if (Helper.PointFValueIsInRange(point, ImageTapAreas[i].X, ImageTapAreas[i].X + ImageTapAreas[i].Width,
                            ImageTapAreas[i].Y, ImageTapAreas[i].Y + ImageTapAreas[i].Height))
                        {
                            tappedImageAreasIndexes.Add(i);
                        }
                    }
                    if (OnMCLImageTapped != null)
                        OnMCLImageTapped(this, new MCLImageEventArgs(e, tappedImageAreasIndexes.ToArray<int>()));
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Image != null)
            {
                canvas.DrawImage(Image, 0, 0, Image.Width, Image.Height);
            }
        }

        public void LoadImage(Assembly assembly, string manifestResourcePath)
        {
            Image = Helper.LoadImage(assembly, manifestResourcePath);
            if(Image != null)
            {
                this.WidthRequest = Image.Width;
                this.HeightRequest = Image.Height;
            }
        }
    }
}
