namespace MauiControlsLibrary
{
    public abstract class MCLProgressBarBase : GraphicsView, IDrawable
    {
        protected decimal minValue = 0;
        public decimal MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                Invalidate();
            }
        }
        protected decimal maxValue = 100;
        public decimal MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                Invalidate();
            }
        }
        protected decimal currentValue = 0;
        public virtual decimal CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = value;
                Invalidate();
            }
        }
        public string? BeforeValueLabel { get; set; } = "";
        public string? AfterValueLabel { get; set; } = "%";
        public bool ArrangeHorizontal { get; set; } = true;
        public Color ProgressBarBackgroundColor { get; set; } = Colors.Grey;
        public int RoundedRectangleRadius { get; set; } = 5;
        public Helper.StandardFontPropterties LabelFont { get; set; } = new();
        public event EventHandler<MCLProgressBarEventArgs>? OnMCLProgressBarTapped;

        public class MCLProgressBarEventArgs(EventArgs? eventArgs, decimal minValue, decimal maxValue, decimal currentValue) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public decimal MinValue { get; set; } = minValue;
            public decimal MaxValue { get; set; } = maxValue;
            public decimal CurrentValue { get; set; } = currentValue;
        }

        public MCLProgressBarBase()
        {
            Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(this);
            if (Helper.PointFValueIsInRange(point, 0, Width, 0, Height))
            {
                OnMCLProgressBarTapped?.Invoke(this, new MCLProgressBarEventArgs(e, minValue, maxValue, currentValue));
                Invalidate();
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)this.Width, (float)this.Height);
            int progressBarFillLengthInPixels = (int)(currentValue * (ArrangeHorizontal ? (decimal)Width : (decimal)Height) / (maxValue - minValue));
            if (currentValue > minValue)
            {
                DrawProgressBar(canvas, 0, (float)(ArrangeHorizontal ? 0 : Height - progressBarFillLengthInPixels), (float)(ArrangeHorizontal ? 
                    progressBarFillLengthInPixels : Width), (float)(ArrangeHorizontal ? Height : progressBarFillLengthInPixels), progressBarFillLengthInPixels);
            }
            DrawProgressBarLabel(canvas, 0, 0, (float)this.Width, (float)this.Height, currentValue);
        }

        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height) { }

        public virtual void DrawProgressBar(ICanvas canvas, float x, float y, float width, float height, int progressBarFillLengthInPixels) { }

        public virtual void DrawProgressBarLabel(ICanvas canvas, float x, float y, float width, float height, decimal currentValue) 
        {
            Helper.SetFontAttributes(canvas, LabelFont);
            canvas.DrawString($"{BeforeValueLabel}{CurrentValue}{AfterValueLabel}", x, y, width, height, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
        }
    }
}
