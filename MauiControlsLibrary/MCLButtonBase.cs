namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a base class for a button in the Maui Controls Library.
    /// </summary>
    public abstract class MCLButtonBase : GraphicsView, IDrawable
    {
        /// <summary>
        /// Occurs when the button is tapped.
        /// </summary>
        public event EventHandler<EventArgs>? OnMCLButtonTapped;

        private bool tapped = false;

        /// <summary>
        /// Gets or sets a value indicating whether the button is tapped.
        /// </summary>
        public bool Tapped { get => tapped; set { tapped = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCLButtonBase"/> class.
        /// </summary>
        public MCLButtonBase()
        {
            Drawable = this;
            Helper.CreateTapGestureRecognizer(TapGestureRecognizer_Tapped, GestureRecognizers);
        }

        /// <summary>
        /// Handles the Tapped event of the TapGestureRecognizer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            ButtonTapped(0, (float)Width, 0, (float)Height, ref tapped, OnMCLButtonTapped, this, Invalidate, e);
        }

        /// <summary>
        /// Handles the button tapped event.
        /// </summary>
        /// <param name="x">The x-coordinate of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="y">The y-coordinate of the button.</param>
        /// <param name="height">The height of the button.</param>
        /// <param name="tapped">The reference to the tapped flag.</param>
        /// <param name="onButtonTapped">The event to invoke when the button is tapped.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="actionInvalidate">The action to invalidate the button.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        public static void ButtonTapped(float x, float width, float y, float height, ref bool tapped,
            EventHandler<EventArgs>? onButtonTapped, GraphicsView? sender, Action actionInvalidate, TappedEventArgs e)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.PointFValueIsInRange(point, x, width, y, height))
            {
                tapped = true;
                onButtonTapped?.Invoke(sender, e);
                actionInvalidate();
            }
        }

        /// <summary>
        /// Draws the button on the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area of the canvas to redraw.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect) 
        {
            DrawButton(canvas, 0, 0, (float)Width, (float)Height);
            ButtonLogic(ref tapped, Invalidate);
        }

        /// <summary>
        /// Draws the button on the specified canvas at the specified location and with the specified size.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the button.</param>
        /// <param name="y">The y-coordinate of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="height">The height of the button.</param>
        public virtual void DrawButton(ICanvas canvas, float x, float y, float width, float height) { }

        /// <summary>
        /// Handles the logic for the button.
        /// </summary>
        /// <param name="tapped">The reference to the tapped flag.</param>
        /// <param name="invalidate">The action to invalidate the button.</param>
        public static void ButtonLogic(ref bool tapped, Action invalidate)
        {
            if (tapped)
            {
                tapped = false;
                invalidate();
            }
        }
    }
}
