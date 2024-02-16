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
                Invalidate();
            }
        }
        private decimal maxValue = 100;
        public decimal MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                Invalidate();
            }
        }
        private decimal currentValue = 0;
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
        public Color ProgressBarColor { get; set; } = Colors.Green;
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

        public MCLProgressBar()
        {
            Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Helper.PointFValueIsInRange(point, 0, Width, 0, Height))
                {
                    OnMCLProgressBarTapped?.Invoke(this, new MCLProgressBarEventArgs(e, minValue, maxValue, currentValue));
                    Invalidate();
                }
            };
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = ProgressBarBackgroundColor;
            canvas.FillRoundedRectangle(new RectF(0, 0, (float)Width, (float)Height), RoundedRectangleRadius);
            int progressBarFillLengthInPixels = (int)(currentValue * (ArrangeHorizontal ? (decimal)Width : (decimal)Height) / (maxValue - minValue));
            if (currentValue > minValue)
            {
                canvas.FillColor = ProgressBarColor;
                canvas.FillRoundedRectangle(new RectF(0, (float)(ArrangeHorizontal ? 0 : Height - progressBarFillLengthInPixels),
                    (float)(ArrangeHorizontal ? progressBarFillLengthInPixels : Width),
                    (float)(ArrangeHorizontal ? Height : progressBarFillLengthInPixels)), RoundedRectangleRadius);
            }
            Helper.SetFontAttributes(canvas, LabelFont);
            canvas.DrawString($"{BeforeValueLabel}{currentValue}{AfterValueLabel}", 0, 0, (float)Width, (float)Height,
                LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
        }
    }
}
