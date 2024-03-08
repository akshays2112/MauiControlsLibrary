namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a button with an icon and a label.
    /// </summary>
    public class MCLIconLabelButton : GraphicsView, IDrawable
    {
        /// <summary>
        /// Gets or sets the label of the button.
        /// </summary>
        public MCLLabel MCLLabel { get; set; } = new();

        /// <summary>
        /// Gets or sets the icon image of the button.
        /// </summary>
        public MCLImage MCLIconImage { get; set; } = new();

        /// <summary>
        /// Gets or sets the width of the label.
        /// </summary>
        public int MCLLabelWidth { get; set; } = 100;

        /// <summary>
        /// Gets or sets the height of the label.
        /// </summary>
        public int MCLLabelHeight { get; set; } = 25;

        /// <summary>
        /// Gets or sets the width of the icon image.
        /// </summary>
        public int MCLIconImageWidth { get; set; } = 25;

        /// <summary>
        /// Gets or sets the height of the icon image.
        /// </summary>
        public int MCLIconImageHeight { get; set; } = 25;

        /// <summary>
        /// Gets or sets the left spacing of the icon image.
        /// </summary>
        public int IconImageLeftSpacing { get; set; } = 20;

        /// <summary>
        /// Gets or sets the color of the button.
        /// </summary>
        public Color ButtonColor { get; set; } = Colors.Green;

        /// <summary>
        /// Gets or sets the color of the button when it is tapped.
        /// </summary>
        public Color ButtonTappedColor { get; set; } = Colors.Red;

        /// <summary>
        /// Gets or sets the left spacing of the label.
        /// </summary>
        public int LabelLeftSpacing { get; set; } = 10;

        /// <summary>
        /// Occurs when the button is tapped.
        /// </summary>
        public event EventHandler<EventArgs>? OnMCLButtonTapped;

        private bool tapped = false;

        /// <summary>
        /// Gets or sets a value indicating whether the button is tapped.
        /// </summary>
        public bool Tapped { get => tapped; set { tapped = value; } }

        public MCLIconLabelButton()
        {
            Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        /// <summary>
        /// Handles the Tapped event of the TapGestureRecognizer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The TappedEventArgs instance containing the event data.</param>
        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            MCLButtonBase.ButtonTapped(0, (float)Width, 0, (float)Height, ref tapped, OnMCLButtonTapped, this, Invalidate, e);
        }

        /// <summary>
        /// Draws the button on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area of the canvas that needs to be redrawn.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            MCLLabel.LabelBackgroundColor = Tapped ? ButtonTappedColor : ButtonColor;
            MCLLabel.DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            MCLButtonBase.ButtonLogic(ref tapped, Invalidate);
            MCLIconImage?.DrawImage(canvas, IconImageLeftSpacing, (float)(Height - MCLIconImageHeight) / 2F,
                (float)MCLIconImageWidth, (float)MCLIconImageHeight);
            MCLLabel?.DrawLabel(canvas, IconImageLeftSpacing + (float)MCLIconImageWidth + LabelLeftSpacing,
                (float)(Height - MCLLabelHeight) / 2F, (float)MCLLabelWidth, (float)MCLLabelHeight);
        }
    }
}
