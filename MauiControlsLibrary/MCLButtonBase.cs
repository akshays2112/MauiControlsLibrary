namespace MauiControlsLibrary
{
    public abstract class MCLButtonBase : GraphicsView, IDrawable
    {
        public event EventHandler<EventArgs>? OnMCLButtonTapped;
        private bool tapped = false;
        public bool Tapped { get => tapped; set { tapped = value; } }

        public MCLButtonBase()
        {
            Drawable = this;
            Helper.CreateTapGestureRecognizer(TapGestureRecognizer_Tapped, GestureRecognizers);
        }

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            ButtonTapped(0, (float)Width, 0, (float)Height, ref tapped, OnMCLButtonTapped, this, Invalidate, e);
        }

        public static void ButtonTapped(float x, float width, float y, float height, ref bool tapped,
            EventHandler<EventArgs>? onButtonTapped, Element? sender, Action actionInvalidate, TappedEventArgs e)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.PointFValueIsInRange(point, x, width, y, height))
            {
                tapped = true;
                onButtonTapped?.Invoke(sender, e);
                actionInvalidate();
            }
        }

        public virtual void Draw(ICanvas canvas, RectF dirtyRect) 
        {
            DrawButton(canvas, 0, 0, (float)Width, (float)Height);
            ButtonLogic(ref tapped, Invalidate);
        }

        public virtual void DrawButton(ICanvas canvas, float x, float y, float width, float height) { }

        public static void ButtonLogic(ref bool tapped, Action invalidate)
        {
            if (tapped)
            {
                tapped = false;
                invalidate();
            }
        }
    }
}
