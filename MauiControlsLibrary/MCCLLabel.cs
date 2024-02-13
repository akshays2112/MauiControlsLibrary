using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiControlsLibrary
{
    public class MCCLLabel : GraphicsView, IDrawable
    {
        private string labelText = string.Empty;
        public string LabelText { 
            get => labelText; 
            set 
            { 
                labelText = value; 
                this.Invalidate(); 
            }
        }
        public Microsoft.Maui.Graphics.Font LabelTextFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color LabelTextColor { get; set; } = Colors.Black;
        public int LabelTextFontSize { get; set; } = 18;
        public HorizontalAlignment LabelTextHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment LabelTextVerticalAlignment { get; set; } = VerticalAlignment.Center;
        public Color? LabelBackgroundColor { get; set; } = null;

        public MCCLLabel()
        {
            this.Drawable = this;
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if(LabelBackgroundColor != null)
            {
                canvas.FillColor = LabelBackgroundColor;
                canvas.FillRectangle(new Rect(0, 0, this.Width, this.Height));
            }
            canvas.Font = LabelTextFont;
            canvas.FontColor = LabelTextColor;
            canvas.FontSize = LabelTextFontSize;
            canvas.DrawString(LabelText, 0, 0, (float)this.Width, (float)this.Height, LabelTextHorizontalAlignment, LabelTextVerticalAlignment);
        }
    }
}
