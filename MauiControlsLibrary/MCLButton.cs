using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLButton : GraphicsView, IDrawable
    {
        public string ButtonText { get; set; } = string.Empty;
        public Color ButtonColor { get; set; } = Colors.Green;
        public Color ButtonTappedColor { get; set; } = Colors.Red;
        public double ButtonCornerRadius { get; set; } = 5;
        public Helper.StandardFontPropterties ButtonLabelFont { get; set; } = new();
        public event EventHandler<EventArgs>? OnMCLButtonTapped;
        public bool Tapped { get; set; } = false;

        public MCLButton()
        {
            Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Helper.PointFValueIsInRange(point, 0, Width, 0, Height))
                {
                    Tapped = true;
                    OnMCLButtonTapped?.Invoke(this, e);
                    Invalidate();
                }
            };
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Tapped)
            {
                Tapped = false;
                DrawButton(canvas, ButtonTappedColor);
                Invalidate();
            }
            else
                DrawButton(canvas, ButtonColor);
        }

        private void DrawButton(ICanvas canvas, Color color)
        {
            canvas.FillColor = color;
            canvas.FillRoundedRectangle(new Rect(0, 0, Width, Height), ButtonCornerRadius);
            if (!string.IsNullOrEmpty(ButtonText))
            {
                Helper.SetFontAttributes(canvas, ButtonLabelFont);
                canvas.DrawString(ButtonText, 0, 0, (float)Width, (float)Height, ButtonLabelFont.HorizontalAlignment,
                    ButtonLabelFont.VerticalAlignment);
            }
        }
    }
}
