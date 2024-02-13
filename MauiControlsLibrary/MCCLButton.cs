using Microsoft.Maui.Graphics;

namespace MauiControlsLibrary
{
    public class MCCLButton : GraphicsView, IDrawable
    {
        public string ButtonText { get; set; } = string.Empty;
        public Microsoft.Maui.Graphics.Font ButtonTextFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color ButtonTextColor { get; set; } = Colors.Black;
        public int ButtonTextFontSize { get; set; } = 18;
        public HorizontalAlignment ButtonTextHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment ButtonTextVerticalAlignment { get; set; } = VerticalAlignment.Center;
        public Color ButtonColor { get; set; } = Colors.Green;
        public Color ButtonTappedColor { get; set; } = Colors.Red;
        public double CornerRadius { get; set; } = 5;
        public event EventHandler<EventArgs>? OnMCCLButtonTapped;
        public bool Tapped { get; set; } = false;

        public MCCLButton()
        {
            this.Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if(point.HasValue && point.Value.X >= 0 && point.Value.X <= this.Width
                    && point.Value.Y >= 0 && point.Value.Y < this.Height)
                {
                    Tapped = true;
                    if (OnMCCLButtonTapped != null)
                        OnMCCLButtonTapped(this, e);
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Tapped)
            {
                Tapped = false;
                DrawButton(canvas, this.ButtonTappedColor);
                this.Invalidate();
            }
            else
            {
                DrawButton(canvas, this.ButtonColor);
            }
        }

        private void DrawButton(ICanvas canvas, Color color)
        {
            canvas.FillColor = color;
            canvas.FillRoundedRectangle(new Rect(0, 0, this.Width, this.Height), CornerRadius);
            canvas.Font = ButtonTextFont;
            canvas.FontColor = ButtonTextColor;
            canvas.FontSize = ButtonTextFontSize;
            canvas.DrawString(ButtonText, 0, 0, (float)this.Width, (float)this.Height, ButtonTextHorizontalAlignment, ButtonTextVerticalAlignment);
        }
    }
}
