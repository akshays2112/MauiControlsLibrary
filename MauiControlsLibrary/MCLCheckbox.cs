using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiControlsLibrary
{
    public class MCLCheckbox : GraphicsView, IDrawable
    {
        public string CheckboxCheckedText { get; set; } = "✓";
        public Microsoft.Maui.Graphics.Font CheckboxTextFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color CheckboxTextColor { get; set; } = Colors.Black;
        public int CheckboxTextFontSize { get; set; } = 18;
        public HorizontalAlignment CheckboxTextHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment CheckboxTextVerticalAlignment { get; set; } = VerticalAlignment.Center;
        public Color CheckboxColor { get; set; } = Colors.Green;
        public Color CheckboxTappedColor { get; set; } = Colors.Red;
        public double CheckboxCornerRadius { get; set; } = 5;
        public event EventHandler<EventArgs>? OnMCLCheckboxChanged;
        public bool IsChecked { get; set; } = false;

        public MCLCheckbox()
        {
            this.Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (point.HasValue && point.Value.X >= 0 && point.Value.X <= this.Width
                    && point.Value.Y >= 0 && point.Value.Y < this.Height)
                {
                    IsChecked = !IsChecked;
                    if (OnMCLCheckboxChanged != null)
                        OnMCLCheckboxChanged(this, e);
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = CheckboxColor;
            canvas.FillRoundedRectangle(new Rect(0, 0, this.Width, this.Height), CheckboxCornerRadius);
            if (IsChecked && !string.IsNullOrEmpty(CheckboxCheckedText))
            {
                Helper.SetFontAttributes(canvas, CheckboxTextFont, CheckboxTextColor, CheckboxTextFontSize);
                canvas.DrawString(CheckboxCheckedText, 0, 0, (float)this.Width, (float)this.Height, CheckboxTextHorizontalAlignment, CheckboxTextVerticalAlignment);
            }
        }
    }
}
