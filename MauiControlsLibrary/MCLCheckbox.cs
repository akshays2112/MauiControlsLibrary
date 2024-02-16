namespace MauiControlsLibrary
{
    public class MCLCheckbox : GraphicsView, IDrawable
    {
        public string CheckboxCheckedText { get; set; } = "✓";
        public Helper.StandardFontPropterties CheckboxFont { get; set; } = new();
        public Color CheckboxColor { get; set; } = Colors.Green;
        public Color CheckboxTappedColor { get; set; } = Colors.Red;
        public double CheckboxCornerRadius { get; set; } = 5;
        public event EventHandler<EventArgs>? OnMCLCheckboxChanged;
        public bool IsChecked { get; set; } = false;

        public MCLCheckbox()
        {
            Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Helper.PointFValueIsInRange(point, 0, Width, 0, Height))
                {
                    IsChecked = !IsChecked;
                    OnMCLCheckboxChanged?.Invoke(this, e);
                    Invalidate();
                }
            };
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = CheckboxColor;
            canvas.FillRoundedRectangle(new Rect(0, 0, Width, Height), CheckboxCornerRadius);
            if (IsChecked && !string.IsNullOrEmpty(CheckboxCheckedText))
            {
                Helper.SetFontAttributes(canvas, CheckboxFont);
                canvas.DrawString(CheckboxCheckedText, 0, 0, (float)Width, (float)Height,
                    CheckboxFont.HorizontalAlignment, CheckboxFont.VerticalAlignment);
            }
        }
    }
}
