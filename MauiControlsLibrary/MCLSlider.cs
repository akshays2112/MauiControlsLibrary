namespace MauiControlsLibrary
{
    public class MCLSlider : GraphicsView, IDrawable
    {
        public MCLSlider()
        {
            Drawable = this;
        }

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {

        }
    }
}
