namespace MauiControlsLibrary
{
    public class MCLCombobox : MCLComboboxBase
    {
        public string ButtonTextForListboxCollapsed { get; set; } = "▼";
        public string ButtonTextForListboxExpanded { get; set; } = "▲";
        public Helper.StandardFontPropterties ButtonFont { get; set; } = new();
        public Color ButtonColor { get; set; } = Colors.Green;
        public Color ButtonTappedColor { get; set; } = Colors.Red;

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
