namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a listbox in the Maui Controls Library.
    /// </summary>
    public class MCLListbox : GraphicsView, IDrawable
    {
        /// <summary>
        /// Gets or sets the labels of the listbox.
        /// </summary>
        public string[] Labels { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets the font properties of the labels.
        /// </summary>
        public Helper.StandardFontPropterties LabelFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);

        /// <summary>
        /// Gets or sets the background color of the labels.
        /// </summary>
        public Color? LabelBackgroundColor { get; set; } = null;

        /// <summary>
        /// Gets or sets the height of the rows.
        /// </summary>
        public int RowHeight { get; set; } = 25;

        /// <summary>
        /// Occurs when the listbox is tapped.
        /// </summary>
        public event EventHandler<ListboxEventArgs>? OnMCLListboxTapped;

        public class ListboxEventArgs(EventArgs? eventArgs, int currentIndex) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public int CurrentIndex { get; set; } = currentIndex;
        }

        private int currentPanY = 0;

        public MCLListbox()
        {
            Drawable = this;
            Helper.CreatePanGestureRecognizer(PanGesture_PanUpdated, GestureRecognizers);
            Helper.CreateTapGestureRecognizer(TapGestureRecognizer_Tapped, GestureRecognizers);
        }

        /// <summary>
        /// Handles the Tapped event of the TapGestureRecognizer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            ListboxTapped(0, (float)Width, 0, (float)Height, currentPanY, RowHeight, Labels, OnMCLListboxTapped, this, e, Invalidate);
        }

        /// <summary>
        /// Handles the PanUpdated event of the PanGestureRecognizer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PanUpdatedEventArgs"/> instance containing the event data.</param>
        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            ListboxPan(Labels, ref currentPanY, RowHeight, e, Invalidate);
        }

        /// <summary>
        /// Handles the tap event for the listbox.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the listbox.</param>
        /// <param name="width">The width of the listbox.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the listbox.</param>
        /// <param name="height">The height of the listbox.</param>
        /// <param name="currentPanY">The current vertical pan position.</param>
        /// <param name="rowHeight">The height of a row in the listbox.</param>
        /// <param name="labels">The labels in the listbox.</param>
        /// <param name="onMCLListboxTapped">The event handler for the tap event.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        /// <param name="invalidate">The action to invalidate the listbox.</param>
        public static void ListboxTapped(float x, float width, float y, float height, int currentPanY, int rowHeight, string[] labels,
            EventHandler<ListboxEventArgs>? onMCLListboxTapped, GraphicsView sender, TappedEventArgs e, Action invalidate)
        {
            Point? point = e.GetPosition(sender);
            if (point.HasValue && Helper.PointFValueIsInRange(point, x, width, y, height))
            {
                int currentIndex = (int)Math.Floor((currentPanY / (decimal)rowHeight) + ((decimal)point.Value.Y / rowHeight));
                currentIndex = Helper.ValueResetOnBoundsCheck(currentIndex, 0, labels.Length, moreThanMaxValueSet: labels.Length - 1);
                onMCLListboxTapped?.Invoke(sender, new ListboxEventArgs(e, currentIndex));
                invalidate();
            }
        }

        /// <summary>
        /// Handles the pan event for the listbox.
        /// </summary>
        /// <param name="labels">The labels in the listbox.</param>
        /// <param name="currentPanY">The current vertical pan position.</param>
        /// <param name="rowHeight">The height of a row in the listbox.</param>
        /// <param name="e">The <see cref="PanUpdatedEventArgs"/> instance containing the event data.</param>
        /// <param name="invalidate">The action to invalidate the listbox.</param>
        public static void ListboxPan(string[] labels, ref int currentPanY, int rowHeight, PanUpdatedEventArgs e, Action invalidate)
        {
            if (labels != null && e.StatusType == GestureStatus.Running)
            {
                currentPanY += (int)e.TotalY;
                currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, (labels.Length - 1) * rowHeight);
                invalidate();
            }
        }

        /// <summary>
        /// Draws the listbox on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area of the canvas that needs to be redrawn.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            if (Helper.ArrayNotNullOrEmpty(Labels))
            {
                canvas.SaveState();
                canvas.ClipRectangle(0, 0, (float)Width, (float)Height);
                int rowStart = (int)Math.Floor(currentPanY / (decimal)RowHeight);
                rowStart = Helper.ValueResetOnBoundsCheck(rowStart, 0, Labels.Length - 1);
                for (int row = rowStart; row < Labels.Length && (row * RowHeight) - currentPanY < Height; row++)
                {
                    DrawListLabel(canvas, row, 0, (row * RowHeight) - currentPanY, (float)Width, RowHeight);
                }
                canvas.ResetState();
            }
        }

        /// <summary>
        /// Draws the frame of the listbox on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the listbox.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the listbox.</param>
        /// <param name="width">The width of the listbox.</param>
        /// <param name="height">The height of the listbox.</param>
        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(new Rect(x, y, width, height));
        }

        /// <summary>
        /// Draws the label of the listbox on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="row">The row of the label.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the label.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the label.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="height">The height of the label.</param>
        public virtual void DrawListLabel(ICanvas canvas, int row, float x, float y, float width, float height)
        {
            if (Labels[row] != null)
            {
                Helper.SetFontAttributes(canvas, LabelFont);
                canvas.DrawString(Labels[row], x, y, width, height, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
            }
        }
    }
}
