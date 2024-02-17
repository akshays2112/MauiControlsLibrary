namespace MauiControlsLibrary
{
    public class MCLButton : MCLButtonBase
    {
        public string ButtonText { get; set; } = string.Empty;
        public Helper.StandardFontPropterties ButtonTextFont { get; set; } = new();
        public Color ButtonColor { get; set; } = Colors.Green;
        public Color ButtonTappedColor { get; set; } = Colors.Red;
        public double ButtonCornerRadius { get; set; } = 5;

        public override void DrawButton(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.FillColor = Tapped ? ButtonTappedColor : ButtonColor;
            canvas.FillRoundedRectangle(new Rect(x, y, width, height), ButtonCornerRadius);
            if (!string.IsNullOrEmpty(ButtonText))
            {
                Helper.SetFontAttributes(canvas, ButtonTextFont);
                canvas.DrawString(ButtonText, x, y, width, height, ButtonTextFont.HorizontalAlignment, ButtonTextFont.VerticalAlignment);
            }
        }
    }
}
