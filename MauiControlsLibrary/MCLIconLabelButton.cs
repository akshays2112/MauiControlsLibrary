namespace MauiControlsLibrary
{
    public class MCLIconLabelButton : GraphicsView, IDrawable
    {
        public MCLLabel MCLLabel { get; set; } = new();
        public MCLImage MCLIconImage { get; set; } = new();
        public int MCLLabelWidth { get; set; } = 100;
        public int MCLLabelHeight { get; set; } = 25;
        public int MCLIconImageWidth { get; set; } = 25;
        public int MCLIconImageHeight { get; set; } = 25;
        public int IconImageLeftSpacing { get; set; } = 20;
        public Color ButtonColor { get; set; } = Colors.Green;
        public Color ButtonTappedColor { get; set; } = Colors.Red;
        public int LabelLeftSpacing { get; set; } = 10;
        public event EventHandler<EventArgs>? OnMCLButtonTapped;
        public bool Tapped { get; set; } = false;

        public MCLIconLabelButton()
        {
            Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(this);
            if (Helper.PointFValueIsInRange(point, 0, Width, 0, Height))
            {
                Tapped = true;
                OnMCLButtonTapped?.Invoke(this, e);
                Invalidate();
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            MCLLabel.LabelBackgroundColor = Tapped? ButtonTappedColor : ButtonColor;
            MCLLabel.DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            if (Tapped)
            {
                Tapped = false;
                this.Invalidate();
            }
            if (MCLIconImage.Image != null)
                MCLIconImage.DrawImage(canvas, IconImageLeftSpacing, (float)(Height - MCLIconImageHeight) / 2F,
                    (float)MCLIconImageWidth, (float)MCLIconImageHeight);
            if(MCLLabel.LabelText != null)
                MCLLabel.DrawLabel(canvas, IconImageLeftSpacing + (float)MCLIconImageWidth + LabelLeftSpacing, 
                    (float)(Height - MCLLabelHeight) / 2F, (float)MCLLabelWidth, (float)MCLLabelHeight);
        }
    }
}
