namespace MauiControlsLibrary
{
    public class MCLCheckbox : MCLCheckboxBase
    {
        public string CheckboxCheckedText { get; set; } = "✓";
        public Helper.StandardFontPropterties CheckboxFont { get; set; } = new();
        public Color CheckboxColor { get; set; } = Colors.Green;
        public Color CheckboxTappedColor { get; set; } = Colors.Red;
        public double CheckboxCornerRadius { get; set; } = 5;

        public override void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.FillColor = CheckboxColor;
            canvas.FillRoundedRectangle(new Rect(x, y, width, height), CheckboxCornerRadius);
        }

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
