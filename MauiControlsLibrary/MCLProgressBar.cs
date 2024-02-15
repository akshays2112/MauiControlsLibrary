using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLProgressBar : GraphicsView, IDrawable
    {
        private decimal minValue = 0;
        public decimal MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                this.Invalidate();
            }
        }
        private decimal maxValue = 100;
        public decimal MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                this.Invalidate();
            }
        }
        private decimal currentValue = 0;
        public decimal CurrentValue 
        { 
            get => currentValue;
            set
            {
                currentValue = value;
                this.Invalidate();
            }
        }
        public string? BeforeValueLabel { get; set; } = "";
        public string? AfterValueLabel { get; set; } = "%";
        public bool ArrangeHorizontal { get; set; } = true;
        public Color ProgressBarBackgroundColor { get; set; } = Colors.Grey;
        public Color ProgressBarColor { get; set; } = Colors.Green;
        public int RoundedRectangleRadius { get; set; } = 5;
        public Helper.StandardFontPropterties LabelFont { get; set; } = new();
        public Microsoft.Maui.Graphics.IImage? ProgressBarBackgroundImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? ProgressBarImage { get; set; }
        public bool ClipProgressBarImage { get; set; } = true;
        public event EventHandler<MCLProgressBarEventArgs>? OnMCLProgressBarTapped;

        public class MCLProgressBarEventArgs
        {
            public EventArgs? EventArgs { get; set; }
            public decimal MinValue { get; set; }
            public decimal MaxValue { get; set; }
            public decimal CurrentValue { get; set; }

            public MCLProgressBarEventArgs(EventArgs? eventArgs, decimal minValue, decimal maxValue, decimal currentValue)
            {
                EventArgs = eventArgs;
                MinValue = minValue;
                MaxValue = maxValue;
                CurrentValue = currentValue;
            }
        }

        public MCLProgressBar() 
        {
            this.Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Helper.PointFValueIsInRange(point, 0, this.Width, 0, this.Height))
                {
                    if (OnMCLProgressBarTapped != null)
                        OnMCLProgressBarTapped(this, new MCLProgressBarEventArgs(e, minValue, maxValue, currentValue));
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if(ProgressBarBackgroundImage != null)
            {
                canvas.DrawImage(ProgressBarImage, 0, 0, (float)this.Width, (float)this.Height);
            }
            else
            {
                canvas.FillColor = ProgressBarBackgroundColor;
                canvas.FillRoundedRectangle(new RectF(0, 0, (float)this.Width, (float)this.Height), RoundedRectangleRadius);
            }
            int progressBarFillLengthInPixels = (int)((currentValue * (ArrangeHorizontal ? (decimal)this.Width : (decimal)this.Height)) / (maxValue - minValue));
            if (currentValue > minValue) 
            {
                if (ProgressBarImage != null)
                {
                    canvas.SaveState();
                    canvas.ClipRectangle(new RectF(0, (float)(ArrangeHorizontal ? 0 : this.Height - progressBarFillLengthInPixels),
                        (float)(ArrangeHorizontal ? progressBarFillLengthInPixels : this.Width),
                        (float)(ArrangeHorizontal ? this.Height : progressBarFillLengthInPixels)));
                    if (ClipProgressBarImage)
                    {
                        canvas.DrawImage(ProgressBarImage, 0, 0, (float)this.Width, (float)this.Height);
                    }
                    else
                    {
                        canvas.DrawImage(ProgressBarImage, 0, (float)(ArrangeHorizontal ? 0 : this.Height - progressBarFillLengthInPixels),
                            (float)(ArrangeHorizontal ? progressBarFillLengthInPixels : this.Width),
                            (float)(ArrangeHorizontal ? this.Height : progressBarFillLengthInPixels));
                    }
                    canvas.ResetState();
                }
                else
                {
                    canvas.FillColor = ProgressBarColor;
                    canvas.FillRoundedRectangle(new RectF(0, (float)(ArrangeHorizontal ? 0 : this.Height - progressBarFillLengthInPixels),
                        (float)(ArrangeHorizontal ? progressBarFillLengthInPixels : this.Width),
                        (float)(ArrangeHorizontal ? this.Height : progressBarFillLengthInPixels)), RoundedRectangleRadius);
                }
            }
            Helper.SetFontAttributes(canvas, LabelFont);
            canvas.DrawString($"{BeforeValueLabel}{currentValue}{AfterValueLabel}", 0, 0, (float)this.Width, (float)this.Height,
                LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
        }

        public void LoadProgressBarImages(Assembly assembly, string? manifestResourcePathBackgroundImage, string? manifestResourcePathProgressBarImage)
        {
            ProgressBarBackgroundImage = Helper.LoadImage(assembly, manifestResourcePathBackgroundImage);
            ProgressBarImage = Helper.LoadImage(assembly, manifestResourcePathProgressBarImage);
        }
    }
}
