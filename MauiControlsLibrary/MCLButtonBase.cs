namespace MauiControlsLibrary
{
    public abstract class MCLButtonBase : GraphicsView, IDrawable
    {
        public event EventHandler<EventArgs>? OnMCLButtonTapped;
        public bool Tapped { get; set; } = false;

        public MCLButtonBase()
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
            DrawButton(canvas, 0, 0, (float)Width, (float)Height);
            if (Tapped)
            {
                Tapped = false;
                Invalidate();
            }
        }

        public virtual void DrawButton(ICanvas canvas, float x, float y, float width, float height) { }
    }
}
