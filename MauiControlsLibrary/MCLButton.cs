namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a button in the Maui Controls Library.
    /// </summary>
    public class MCLButton : MCLButtonBase
    {
        /// <summary>
        /// Gets or sets the text of the button.
        /// </summary>
        public string ButtonText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the font properties of the button text.
        /// </summary>
        public Helper.StandardFontPropterties ButtonTextFont { get; set; } = new();

        /// <summary>
        /// Gets or sets the color of the button.
        /// </summary>
        public Color ButtonColor { get; set; } = Colors.Green;

        /// <summary>
        /// Gets or sets the color of the button when it is tapped.
        /// </summary>
        public Color ButtonTappedColor { get; set; } = Colors.Red;

        /// <summary>
        /// Gets or sets the corner radius of the button.
        /// </summary>
        public double ButtonCornerRadius { get; set; } = 5;

        /// <summary>
        /// Draws the button on the specified canvas at the specified location and with the specified size.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the button.</param>
        /// <param name="y">The y-coordinate of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="height">The height of the button.</param>
        public override void DrawButton(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.FillColor = Tapped ? ButtonTappedColor : ButtonColor;
            canvas.FillRoundedRectangle(new RectF(x, y, width, height), ButtonCornerRadius);
            if (!string.IsNullOrEmpty(ButtonText) && ButtonTextFont != null)
            {
                Helper.SetFontAttributes(canvas, ButtonTextFont);
                canvas.DrawString(ButtonText, x, y, width, height, ButtonTextFont.HorizontalAlignment, ButtonTextFont.VerticalAlignment);
            }
        }
    }
}
