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
            if (LabelBackgroundColor != null)
            {
                canvas.FillColor = LabelBackgroundColor;
                canvas.FillRectangle(new Rect(0, 0, Width, Height));
            }
            if (!string.IsNullOrEmpty(LabelText))
            {
                Helper.SetFontAttributes(canvas, LabelFont);
                canvas.DrawString(LabelText, 0, 0, (float)Width, (float)Height, LabelFont.HorizontalAlignment,
                    LabelFont.VerticalAlignment);
            }
        }
    }
}
