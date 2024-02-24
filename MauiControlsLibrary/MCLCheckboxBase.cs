namespace MauiControlsLibrary
{
    public abstract class MCLCheckboxBase : GraphicsView, IDrawable
    {
        public event EventHandler<EventArgs>? OnMCLCheckboxChanged;
        private bool isChecked = false;
        public bool IsChecked { get => isChecked; set { isChecked = value; } }

        public MCLCheckboxBase()
        {
            Drawable = this;
            Helper.CreateTapGestureRecognizer(TapGestureRecognizer_Tapped, GestureRecognizers);
        }

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            CheckboxTapped(0, (float)Width, 0, (float)Height, ref isChecked, OnMCLCheckboxChanged, this, Invalidate, e); 
        }

        public static void CheckboxTapped(float x, float width, float y, float height, ref bool isChecked,
            EventHandler<EventArgs>? onCheckboxChanged, GraphicsView? sender, Action actionInvalidate, TappedEventArgs e)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.PointFValueIsInRange(point, 0, width, 0, height))
            {
                isChecked = !isChecked;
                onCheckboxChanged?.Invoke(sender, e);
                actionInvalidate();
            }
        }

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)this.Width, (float)this.Height);
            DrawCheckmark(canvas, 0, 0, (float)this.Width, (float)this.Height);
        }

        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height) { }

        public virtual void DrawCheckmark(ICanvas canvas, float x, float y, float width, float height) { }
    }
}
