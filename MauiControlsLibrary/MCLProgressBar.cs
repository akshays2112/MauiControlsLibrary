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
        public Microsoft.Maui.Graphics.Font LabelFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color LabelColor { get; set; } = Colors.Black;
        public int LabelFontSize { get; set; } = 18;
        public HorizontalAlignment LabelHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment LabelVerticalAlignment { get; set; } = VerticalAlignment.Center;
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
                if (point.HasValue && point.Value.X >= 0 && point.Value.X <= this.Width
                    && point.Value.Y >= 0 && point.Value.Y < this.Height)
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
            Helper.SetFontAttributes(canvas, LabelFont, LabelColor, LabelFontSize);
            canvas.DrawString($"{BeforeValueLabel}{currentValue}{AfterValueLabel}", 0, 0, (float)this.Width, (float)this.Height,
                LabelHorizontalAlignment, LabelVerticalAlignment);
        }

        public void LoadProgressBarImages(Assembly assembly, string? manifestResourcePathBackgroundImage, string? manifestResourcePathProgressBarImage)
        {
            if (!string.IsNullOrEmpty(manifestResourcePathBackgroundImage))
            {
                ProgressBarBackgroundImage = Helper.LoadImage(assembly, manifestResourcePathBackgroundImage);
            }
            if (!string.IsNullOrEmpty(manifestResourcePathProgressBarImage))
            {
                ProgressBarImage = Helper.LoadImage(assembly, manifestResourcePathProgressBarImage);
            }
        }
    }
}
