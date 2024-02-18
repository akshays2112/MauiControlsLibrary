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
        private bool tapped = false;
        public bool Tapped { get => tapped; set { tapped = value; } }

        public MCLIconLabelButton()
        {
            Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            MCLButtonBase.ButtonTapped(0, (float)Width, 0, (float)Height, ref tapped, OnMCLButtonTapped, this, Invalidate, e);
        }

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            MCLLabel.LabelBackgroundColor = Tapped? ButtonTappedColor : ButtonColor;
            MCLLabel.DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            MCLButtonBase.ButtonLogic(ref tapped, Invalidate);
            MCLIconImage?.DrawImage(canvas, IconImageLeftSpacing, (float)(Height - MCLIconImageHeight) / 2F,
                (float)MCLIconImageWidth, (float)MCLIconImageHeight);
            MCLLabel?.DrawLabel(canvas, IconImageLeftSpacing + (float)MCLIconImageWidth + LabelLeftSpacing, 
                (float)(Height - MCLLabelHeight) / 2F, (float)MCLLabelWidth, (float)MCLLabelHeight);
        }
    }
}
