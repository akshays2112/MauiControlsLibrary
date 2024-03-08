namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a base class for a checkbox in the Maui Controls Library.
    /// </summary>
    public abstract class MCLCheckboxBase : GraphicsView, IDrawable
    {
        /// <summary>
        /// Occurs when the checkbox state is changed.
        /// </summary>
        public event EventHandler<EventArgs>? OnMCLCheckboxChanged;

        private bool isChecked = false;

        /// <summary>
        /// Gets or sets a value indicating whether the checkbox is checked.
        /// </summary>
        public bool IsChecked { get => isChecked; set { isChecked = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCLCheckboxBase"/> class.
        /// </summary>
        public MCLCheckboxBase()
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
            CheckboxTapped(0, (float)Width, 0, (float)Height, ref isChecked, OnMCLCheckboxChanged, this, Invalidate, e);
        }

        /// <summary>
        /// Handles the checkbox tapped event.
        /// </summary>
        /// <param name="x">The x-coordinate of the checkbox.</param>
        /// <param name="width">The width of the checkbox.</param>
        /// <param name="y">The y-coordinate of the checkbox.</param>
        /// <param name="height">The height of the checkbox.</param>
        /// <param name="isChecked">The reference to the checked flag.</param>
        /// <param name="onCheckboxChanged">The event to invoke when the checkbox is tapped.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="actionInvalidate">The action to invalidate the checkbox.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        public static void CheckboxTapped(float x, float width, float y, float height, ref bool isChecked,
            EventHandler<EventArgs>? onCheckboxChanged, GraphicsView? sender, Action actionInvalidate, TappedEventArgs e)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.PointFValueIsInRange(point, 0, width, 0, height))
            {
                isChecked = !isChecked;
                onCheckboxChanged?.Invoke(sender, e);
                actionInvalidate();
            }
        }

        /// <summary>
        /// Draws the checkbox on the specified canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area of the canvas to redraw.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)this.Width, (float)this.Height);
            DrawCheckmark(canvas, 0, 0, (float)this.Width, (float)this.Height);
        }

        /// <summary>
        /// Draws the frame of the checkbox on the specified canvas at the specified location and with the specified size.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the checkbox.</param>
        /// <param name="y">The y-coordinate of the checkbox.</param>
        /// <param name="width">The width of the checkbox.</param>
        /// <param name="height">The height of the checkbox.</param>
        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height) { }

        /// <summary>
        /// Draws the checkmark of the checkbox on the specified canvas at the specified location and with the specified size.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the checkbox.</param>
        /// <param name="y">The y-coordinate of the checkbox.</param>
        /// <param name="width">The width of the checkbox.</param>
        /// <param name="height">The height of the checkbox.</param>
        public virtual void DrawCheckmark(ICanvas canvas, float x, float y, float width, float height) { }
    }
}
