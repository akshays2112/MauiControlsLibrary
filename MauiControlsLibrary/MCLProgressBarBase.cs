namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a base class for a progress bar in the Maui Controls Library.
    /// </summary>
    public abstract class MCLProgressBarBase : GraphicsView, IDrawable
    {
        protected decimal minValue = 0;

        /// <summary>
        /// Gets or sets the minimum value of the progress bar.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the maximum value of the progress bar.
        /// </summary>
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

        /// <summary>
        /// Gets or sets the current value of the progress bar.
        /// </summary>
        public virtual decimal CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the label before the value of the progress bar.
        /// </summary>
        public string? BeforeValueLabel { get; set; } = "";

        /// <summary>
        /// Gets or sets the label after the value of the progress bar.
        /// </summary>
        public string? AfterValueLabel { get; set; } = "%";

        /// <summary>
        /// Gets or sets a value indicating whether the progress bar is arranged horizontally.
        /// </summary>
        public bool ArrangeHorizontal { get; set; } = true;

        /// <summary>
        /// Gets or sets the background color of the progress bar.
        /// </summary>
        public Color ProgressBarBackgroundColor { get; set; } = Colors.Grey;

        /// <summary>
        /// Gets or sets the radius of the rounded rectangle of the progress bar.
        /// </summary>
        public int RoundedRectangleRadius { get; set; } = 5;

        /// <summary>
        /// Gets or sets the font properties of the label of the progress bar.
        /// </summary>
        public Helper.StandardFontPropterties LabelFont { get; set; } = new();

        /// <summary>
        /// Occurs when the progress bar is tapped.
        /// </summary>
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
            Helper.CreateTapGestureRecognizer(TapGestureRecognizer_Tapped, GestureRecognizers);
        }

        /// <summary>
        /// Handles the Tapped event of the TapGestureRecognizer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            ProgressBarTapped(0, (float)Width, 0, (float)Height, OnMCLProgressBarTapped, this, e, Invalidate, minValue, maxValue, currentValue);
        }

        /// <summary>
        /// Handles the tap event for the progress bar.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="width">The width of the progress bar.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="height">The height of the progress bar.</param>
        /// <param name="onMCLProgressBarTapped">The event handler for the tap event.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        /// <param name="invalidate">The action to invalidate the progress bar.</param>
        /// <param name="minValue">The minimum value of the progress bar.</param>
        /// <param name="maxValue">The maximum value of the progress bar.</param>
        /// <param name="currentValue">The current value of the progress bar.</param>
        public static void ProgressBarTapped(float x, float width, float y, float height, EventHandler<MCLProgressBarEventArgs>? onMCLProgressBarTapped,
            GraphicsView sender, TappedEventArgs e, Action invalidate, decimal minValue, decimal maxValue, decimal currentValue)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.PointFValueIsInRange(point, x, width, y, height))
            {
                onMCLProgressBarTapped?.Invoke(sender, new MCLProgressBarEventArgs(e, minValue, maxValue, currentValue));
                invalidate();
            }
        }

        /// <summary>
        /// Draws the progress bar on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area of the canvas that needs to be redrawn.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
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

        /// <summary>
        /// Draws the frame of the progress bar on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="width">The width of the progress bar.</param>
        /// <param name="height">The height of the progress bar.</param>
        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height) { }

        /// <summary>
        /// Draws the progress bar on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the progress bar.</param>
        /// <param name="width">The width of the progress bar.</param>
        /// <param name="height">The height of the progress bar.</param>
        /// <param name="progressBarFillLengthInPixels">The fill length of the progress bar in pixels.</param>
        public virtual void DrawProgressBar(ICanvas canvas, float x, float y, float width, float height, int progressBarFillLengthInPixels) { }

        /// <summary>
        /// Draws the label of the progress bar on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the label.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the label.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="height">The height of the label.</param>
        /// <param name="currentValue">The current value of the progress bar.</param>
        public virtual void DrawProgressBarLabel(ICanvas canvas, float x, float y, float width, float height, decimal currentValue)
        {
            Helper.SetFontAttributes(canvas, LabelFont);
            canvas.DrawString($"{BeforeValueLabel}{CurrentValue}{AfterValueLabel}", x, y, width, height, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
        }
    }
}
