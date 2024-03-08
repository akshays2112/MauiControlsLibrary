namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a combobox in the Maui Controls Library.
    /// </summary>
    public class MCLCombobox : MCLComboboxBase
    {
        /// <summary>
        /// Gets or sets the text to display on the button when the listbox is collapsed.
        /// </summary>
        public string ButtonTextForListboxCollapsed { get; set; } = "▼";

        /// <summary>
        /// Gets or sets the text to display on the button when the listbox is expanded.
        /// </summary>
        public string ButtonTextForListboxExpanded { get; set; } = "▲";

        /// <summary>
        /// Gets or sets the font properties of the button text.
        /// </summary>
        public Helper.StandardFontPropterties ButtonFont { get; set; } = new();

        /// <summary>
        /// Gets or sets the color of the button.
        /// </summary>
        public Color ButtonColor { get; set; } = Colors.Green;

        /// <summary>
        /// Gets or sets the color of the button when it is tapped.
        /// </summary>
        public Color ButtonTappedColor { get; set; } = Colors.Red;

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
            canvas.FillColor = ButtonTapped ? ButtonTappedColor : ButtonColor;
            canvas.FillRoundedRectangle(new RectF(x, y, width, height), ButtonCornerRadius);
            if (!string.IsNullOrEmpty(ButtonTextForListboxCollapsed))
            {
                Helper.SetFontAttributes(canvas, ButtonFont);
                canvas.DrawString(ListboxVisible ? ButtonTextForListboxExpanded : ButtonTextForListboxCollapsed,
                    x, y, width, height, ButtonFont.HorizontalAlignment, ButtonFont.VerticalAlignment);
            }
        }
    }
}
