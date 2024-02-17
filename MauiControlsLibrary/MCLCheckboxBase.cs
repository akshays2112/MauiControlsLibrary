namespace MauiControlsLibrary
{
    public abstract class MCLCheckboxBase : GraphicsView, IDrawable
    {
        public event EventHandler<EventArgs>? OnMCLCheckboxChanged;
        public bool IsChecked { get; set; } = false;

        public MCLCheckboxBase()
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
                IsChecked = !IsChecked;
                OnMCLCheckboxChanged?.Invoke(this, e);
                Invalidate();
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)this.Width, (float)this.Height);
            DrawCheckmark(canvas, 0, 0, (float)this.Width, (float)this.Height);
        }

        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height) { }

        public virtual void DrawCheckmark(ICanvas canvas, float x, float y, float width, float height) { }
    }
}
