namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a checkbox in the Maui Controls Library.
    /// </summary>
    public class MCLCheckbox : MCLCheckboxBase
    {
        /// <summary>
        /// Gets or sets the text to display when the checkbox is checked.
        /// </summary>
        public string CheckboxCheckedText { get; set; } = "✓";

        /// <summary>
        /// Gets or sets the font properties of the checkbox text.
        /// </summary>
        public Helper.StandardFontPropterties CheckboxFont { get; set; } = new();

        /// <summary>
        /// Gets or sets the color of the checkbox.
        /// </summary>
        public Color CheckboxColor { get; set; } = Colors.Green;

        /// <summary>
        /// Gets or sets the color of the checkbox when it is tapped.
        /// </summary>
        public Color CheckboxTappedColor { get; set; } = Colors.Red;

        /// <summary>
        /// Gets or sets the corner radius of the checkbox.
        /// </summary>
        public double CheckboxCornerRadius { get; set; } = 5;

        /// <summary>
        /// Draws the frame of the checkbox on the specified canvas at the specified location and with the specified size.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the checkbox.</param>
        /// <param name="y">The y-coordinate of the checkbox.</param>
        /// <param name="width">The width of the checkbox.</param>
        /// <param name="height">The height of the checkbox.</param>
        public override void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.FillColor = CheckboxColor;
            canvas.FillRoundedRectangle(new Rect(x, y, width, height), CheckboxCornerRadius);
        }

        /// <summary>
        /// Draws the checkmark of the checkbox on the specified canvas at the specified location and with the specified size.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the checkbox.</param>
        /// <param name="y">The y-coordinate of the checkbox.</param>
        /// <param name="width">The width of the checkbox.</param>
        /// <param name="height">The height of the checkbox.</param>
        public override void DrawCheckmark(ICanvas canvas, float x, float y, float width, float height)
        {
            if (IsChecked && !string.IsNullOrEmpty(CheckboxCheckedText))
            {
                Helper.SetFontAttributes(canvas, CheckboxFont);
                canvas.DrawString(CheckboxCheckedText, x, y, width, height, CheckboxFont.HorizontalAlignment, CheckboxFont.VerticalAlignment);
            }
        }
    }
}
