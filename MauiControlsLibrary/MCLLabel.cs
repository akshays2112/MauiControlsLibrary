namespace MauiControlsLibrary
{
    public class MCLLabel : GraphicsView, IDrawable
    {
        private string labelText = string.Empty;
        public string LabelText
        {
            get => labelText;
            set
            {
                labelText = value;
                Invalidate();
            }
        }
        public Helper.StandardFontPropterties LabelFont { get; set; } = new();
        public Color? LabelBackgroundColor { get; set; } = null;

        public MCLLabel()
        {
            Drawable = this;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            if (!string.IsNullOrEmpty(LabelText))
                DrawLabel(canvas, LabelText, 0, 0, (float)Width, (float)Height);
        }

        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height) 
        {
            if (LabelBackgroundColor != null)
            {
                canvas.FillColor = LabelBackgroundColor;
                canvas.FillRectangle(new Rect(x, y, width, height));
            }
        }

        public virtual void DrawLabel(ICanvas canvas, string labelText, float x, float y, float width, float height)
        {
            Helper.SetFontAttributes(canvas, LabelFont);
            canvas.DrawString(labelText, x, y, width, height, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
        }
    }
}
