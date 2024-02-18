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
        public int CornerRadius { get; set; } = 5;

        public MCLLabel()
        {
            Drawable = this;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            DrawLabel(canvas, 0, 0, (float)Width, (float)Height);
        }

        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height) 
        {
            if (LabelBackgroundColor != null)
            {
                canvas.FillColor = LabelBackgroundColor;
                canvas.FillRoundedRectangle(new RectF(x, y, width, height), CornerRadius);
            }
        }

        public virtual void DrawLabel(ICanvas canvas, float x, float y, float width, float height)
        {
            if (!string.IsNullOrEmpty(LabelText) && LabelFont != null)
            {
                Helper.SetFontAttributes(canvas, LabelFont);
                canvas.DrawString(LabelText, x, y, width, height, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
            }
        }
    }
}
