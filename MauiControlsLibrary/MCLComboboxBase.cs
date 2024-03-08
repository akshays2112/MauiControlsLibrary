namespace MauiControlsLibrary
{
    /// <summary>
    /// Base class for a combobox control.
    /// </summary>
    public abstract class MCLComboboxBase : GraphicsView, IDrawable
    {
        /// <summary>
        /// Gets or sets the font properties for the textbox.
        /// </summary>
        public Helper.StandardFontPropterties TextboxFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);

        /// <summary>
        /// Gets or sets the width of the button.
        /// </summary>
        public int ButtonWidth { get; set; } = 25;

        /// <summary>
        /// Gets or sets the height of the button.
        /// </summary>
        public int ButtonHeight { get; set; } = 25;

        private bool buttonTapped = false;

        /// <summary>
        /// Gets or sets a value indicating whether the button is tapped.
        /// </summary>
        public bool ButtonTapped { get => buttonTapped; set { buttonTapped = value; } }

        /// <summary>
        /// Gets or sets the corner radius of the button.
        /// </summary>
        public int ButtonCornerRadius { get; set; } = 5;

        private bool listboxVisible = false;

        /// <summary>
        /// Gets or sets a value indicating whether the listbox is visible.
        /// </summary>
        public bool ListboxVisible { get => listboxVisible; set { listboxVisible = value; } }

        /// <summary>
        /// Gets or sets the labels for the dropdown.
        /// </summary>
        public string[] DropdownLabels { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets the font properties for the label.
        /// </summary>
        public Helper.StandardFontPropterties LabelFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);

        /// <summary>
        /// Gets or sets the background color for the label.
        /// </summary>
        public Color? LabelBackgroundColor { get; set; } = null;

        /// <summary>
        /// Gets or sets the height of the listbox.
        /// </summary>
        public int ListboxHeight { get; set; } = 200;

        /// <summary>
        /// Gets or sets the height of the row.
        /// </summary>
        public int RowHeight { get; set; } = 25;

        private int selectedItemIndex = -1;

        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
        public int SelectedItemIndex { get => selectedItemIndex; set { selectedItemIndex = value; } }

        /// <summary>
        /// Occurs when the selected item changes.
        /// </summary>
        public event EventHandler<ComboboxEventArgs>? SelectedItemChanged;

        protected int currentPanY = 0;

        /// <summary>
        /// Represents the arguments for a combobox event.
        /// </summary>
        public class ComboboxEventArgs(EventArgs? eventArgs, int selectedIndex) : EventArgs
        {
            /// <summary>
            /// Gets or sets the original event arguments.
            /// </summary>
            public EventArgs? EventArgs { get; set; } = eventArgs;

            /// <summary>
            /// Gets or sets the selected index.
            /// </summary>
            public int SelectedIndex { get; set; } = selectedIndex;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCLComboboxBase"/> class.
        /// </summary>
        public MCLComboboxBase()
        {
            Drawable = this;
            Helper.CreateTapGestureRecognizer(TapGestureRecognizerButton_Tapped, GestureRecognizers);
            Helper.CreatePanGestureRecognizer(PanGesture_PanUpdated, GestureRecognizers);
        }

        /// <summary>
        /// Handles the pan gesture update event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PanUpdatedEventArgs"/> instance containing the event data.</param>
        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            ComboboxPanGesture(DropdownLabels, e.StatusType, ref currentPanY, 0, (DropdownLabels.Length * RowHeight) - ButtonHeight,
                e.TotalY, RowHeight, Invalidate);
        }

        /// <summary>
        /// Handles the pan gesture for the combobox.
        /// </summary>
        /// <param name="dropdownLabels">The dropdown labels.</param>
        /// <param name="gestureStatus">The gesture status.</param>
        /// <param name="currentPanY">The current pan Y.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        /// <param name="totalY">The total Y.</param>
        /// <param name="rowHeight">Height of the row.</param>
        /// <param name="invalidate">The invalidate action.</param>
        public static void ComboboxPanGesture(string[] dropdownLabels, GestureStatus gestureStatus, ref int currentPanY,
            int minValue, int maxValue, double totalY, int rowHeight, Action invalidate)
        {
            if (Helper.ArrayNotNullOrEmpty(dropdownLabels) && gestureStatus == GestureStatus.Running)
            {
                currentPanY += (int)totalY - rowHeight;
                currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, minValue, maxValue);
                invalidate();
            }
        }

        /// <summary>
        /// Handles the tap gesture on the button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        public virtual void TapGestureRecognizerButton_Tapped(object? sender, TappedEventArgs e)
        {
            if (!ComboboxDropdownButtonTapped((int)Width - ButtonWidth, (int)Width, 0, ButtonHeight, ref buttonTapped,
                ref listboxVisible, ButtonHeight, ListboxHeight, (GraphicsView)this, Invalidate, e))
                ComboboxListboxLabelTapped(0, (float)Width, (float)Height, ref listboxVisible, DropdownLabels, RowHeight, ref currentPanY,
                    ref selectedItemIndex, ButtonHeight, ListboxHeight, (GraphicsView)this, Invalidate, e, SelectedItemChanged);
        }

        /// <summary>
        /// Handles the tap gesture on the dropdown button.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="height">The height.</param>
        /// <param name="buttonTapped">if set to <c>true</c> [button tapped].</param>
        /// <param name="listboxVisible">if set to <c>true</c> [listbox visible].</param>
        /// <param name="buttonHeight">Height of the button.</param>
        /// <param name="listboxHeight">Height of the listbox.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="invalidate">The invalidate action.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        /// <returns><c>true</c> if the dropdown button was tapped, <c>false</c> otherwise.</returns>
        public static bool ComboboxDropdownButtonTapped(float x, float width, float y, float height, ref bool buttonTapped, ref bool listboxVisible,
            int buttonHeight, int listboxHeight, GraphicsView? sender, Action invalidate, TappedEventArgs e)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.PointFValueIsInRange(point, x, width, y, height) && sender != null)
            {
                buttonTapped = true;
                listboxVisible = !listboxVisible;
                sender.HeightRequest = listboxVisible ? buttonHeight + listboxHeight : buttonHeight;
                invalidate();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Handles the tap gesture on the listbox label.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="listboxVisible">if set to <c>true</c> [listbox visible].</param>
        /// <param name="dropdownLabels">The dropdown labels.</param>
        /// <param name="rowHeight">Height of the row.</param>
        /// <param name="currentPanY">The current pan Y.</param>
        /// <param name="selectedItemIndex">Index of the selected item.</param>
        /// <param name="buttonHeight">Height of the button.</param>
        /// <param name="listboxHeight">Height of the listbox.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="invalidate">The invalidate action.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        /// <param name="selectedItemChanged">The selected item changed event handler.</param>
        public static void ComboboxListboxLabelTapped(float x, float width, float height, ref bool listboxVisible,
            string[] dropdownLabels, int rowHeight, ref int currentPanY, ref int selectedItemIndex, int buttonHeight,
            int listboxHeight, GraphicsView? sender, Action invalidate, TappedEventArgs e, EventHandler<ComboboxEventArgs>? selectedItemChanged)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.ArrayNotNullOrEmpty(dropdownLabels) && point.HasValue && Helper.IntValueIsInRange((int)point.Value.X, (int)x, (int)width)
                && Helper.IntValueIsInRange((int)point.Value.Y, rowHeight, (int)height) && sender != null)
            {
                int currentRowIndex = (int)Math.Floor(((currentPanY - rowHeight) / (decimal)rowHeight) + ((decimal)point.Value.Y / rowHeight));
                currentRowIndex = Helper.ValueResetOnBoundsCheck(currentRowIndex, (int)x, dropdownLabels.Length - 1);
                selectedItemIndex = currentRowIndex;
                listboxVisible = !listboxVisible;
                sender.HeightRequest = listboxVisible ? buttonHeight + listboxHeight : buttonHeight;
                selectedItemChanged?.Invoke(sender, new ComboboxEventArgs(e, currentRowIndex));
                invalidate();
            }
        }

        /// <summary>
        /// Draws the control.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRect">The dirty rectangle.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)Width, ButtonHeight);
            DrawTextbox(canvas, 0, 0, (float)Width - ButtonHeight, ButtonHeight);
            DrawButton(canvas, (float)Width - ButtonHeight, 0, ButtonHeight, ButtonHeight);
            if (ButtonTapped)
            {
                ButtonTapped = false;
                Invalidate();
            }
            if (ListboxVisible && Helper.ArrayNotNullOrEmpty(DropdownLabels))
            {
                DrawListboxFrame(canvas, 0, ButtonHeight, (float)Width, (float)Height - ButtonHeight);
                canvas.SaveState();
                canvas.ClipRectangle(0, ButtonHeight, (float)Width, (float)Height - ButtonHeight);
                int rowStart = (int)Math.Floor(currentPanY / (decimal)RowHeight) - 1;
                rowStart = Helper.ValueResetOnBoundsCheck(rowStart, 0, DropdownLabels.Length - 1);
                for (int row = rowStart; row < DropdownLabels.Length && (row * RowHeight) - currentPanY + RowHeight < Height; row++)
                {
                    if (DropdownLabels[row] != null)
                    {
                        DrawListboxLabel(canvas, row, 0, (row * RowHeight) - currentPanY + RowHeight, (float)Width, RowHeight);
                    }
                }
                canvas.ResetState();
            }
        }

        /// <summary>
        /// Draws the frame of the combobox.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the frame.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the frame.</param>
        /// <param name="width">The width of the frame.</param>
        /// <param name="height">The height of the frame.</param>
        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRoundedRectangle(x, y, width, height, ButtonCornerRadius);
        }

        /// <summary>
        /// Draws the textbox of the combobox.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the textbox.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the textbox.</param>
        /// <param name="width">The width of the textbox.</param>
        /// <param name="height">The height of the textbox.</param>
        public virtual void DrawTextbox(ICanvas canvas, float x, float y, float width, float height)
        {
            if (SelectedItemIndex >= 0 && SelectedItemIndex < DropdownLabels.Length)
            {
                Helper.SetFontAttributes(canvas, TextboxFont);
                canvas.DrawString(DropdownLabels[SelectedItemIndex], x, y, width, height, TextboxFont.HorizontalAlignment,
                    TextboxFont.VerticalAlignment);
            }
        }

        /// <summary>
        /// Draws the button of the combobox.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the button.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="height">The height of the button.</param>
        public virtual void DrawButton(ICanvas canvas, float x, float y, float width, float height) { }

        /// <summary>
        /// Draws the frame of the listbox.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the listbox frame.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the listbox frame.</param>
        /// <param name="width">The width of the listbox frame.</param>
        /// <param name="height">The height of the listbox frame.</param>
        public virtual void DrawListboxFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(new Rect(x, y, width, height));
        }

        /// <summary>
        /// Draws the label of the listbox.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="row">The row index of the label.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the label.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the label.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="height">The height of the label.</param>
        public virtual void DrawListboxLabel(ICanvas canvas, int row, float x, float y, float width, float height)
        {
            Helper.SetFontAttributes(canvas, LabelFont);
            canvas.DrawString(DropdownLabels[row], x, y, width, height, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
        }

        /// <summary>
        /// Handles the event when the dropdown button of the combobox is tapped.
        /// </summary>
        public virtual void ComboboxDropdownButtonTapped() { }
    }
}
