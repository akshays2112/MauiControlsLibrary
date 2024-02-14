using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImage : GraphicsView, IDrawable
    {
        public Microsoft.Maui.Graphics.IImage? Image { get; set; }
        public string ImageTitle { get; set; } = string.Empty;
        public event EventHandler<EventArgs>? OnMCLImageTapped;

        public MCLImage()
        {
            this.Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                if (OnMCLImageTapped != null)
                    OnMCLImageTapped(this, e);
                this.Invalidate();
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

        public void LoadImage(Assembly assembly, string path)
        {
            using (Stream? stream = assembly.GetManifestResourceStream(path))
            {
                Image = PlatformImage.FromStream(stream);
                if(Image != null)
                {
                    this.WidthRequest = Image.Width;
                    this.HeightRequest = Image.Height;
                }
            }
        }
    }
}
