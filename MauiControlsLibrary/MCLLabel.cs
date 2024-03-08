namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a label in the Maui Controls Library.
    /// </summary>
    public class MCLLabel : GraphicsView, IDrawable
    {
        private string labelText = string.Empty;

        /// <summary>
        /// Gets or sets the text of the label.
        /// </summary>
        public string LabelText
        {
            get => labelText;
            set
            {
                labelText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the font properties of the label.
        /// </summary>
        public Helper.StandardFontPropterties LabelFont { get; set; } = new();

        /// <summary>
        /// Gets or sets the background color of the label.
        /// </summary>
        public Color? LabelBackgroundColor { get; set; } = null;

        /// <summary>
        /// Gets or sets the corner radius of the label.
        /// </summary>
        public int CornerRadius { get; set; } = 5;

        public MCLLabel()
        {
            Drawable = this;
        }

        /// <summary>
        /// Draws the label on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area of the canvas that needs to be redrawn.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            DrawLabel(canvas, 0, 0, (float)Width, (float)Height);
        }

        /// <summary>
        /// Draws the frame of the label on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the label.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the label.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="height">The height of the label.</param>
        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            if (LabelBackgroundColor != null)
            {
                canvas.FillColor = LabelBackgroundColor;
                canvas.FillRoundedRectangle(new RectF(x, y, width, height), CornerRadius);
            }
        }

        /// <summary>
        /// Draws the text of the label on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the label.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the label.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="height">The height of the label.</param>
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
