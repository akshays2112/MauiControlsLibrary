namespace MauiControlsLibrary
{
    public abstract class MCLComboboxBase : GraphicsView, IDrawable
    {
        public Helper.StandardFontPropterties TextboxFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);
        public int ButtonWidth { get; set; } = 25;
        public int ButtonHeight { get; set; } = 25;
        private bool buttonTapped = false;
        public bool ButtonTapped { get => buttonTapped; set { buttonTapped = value; } }
        public int ButtonCornerRadius { get; set; } = 5;
        private bool listboxVisible = false;
        public bool ListboxVisible { get => listboxVisible; set { listboxVisible = value; } }
        public string[] DropdownLabels { get; set; } = Array.Empty<string>();
        public Helper.StandardFontPropterties LabelFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);
        public Color? LabelBackgroundColor { get; set; } = null;
        public int ListboxHeight { get; set; } = 200;
        public int RowHeight { get; set; } = 25;
        private int selectedItemIndex = -1;
        public int SelectedItemIndex { get => selectedItemIndex; set { selectedItemIndex = value; } }
        public event EventHandler<ComboboxEventArgs>? SelectedItemChanged;

        protected int currentPanY = 0;

        public class ComboboxEventArgs(EventArgs? eventArgs, int selectedIndex) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public int SelectedIndex { get; set; } = selectedIndex;
        }

        public MCLComboboxBase()
        {
            Drawable = this;
            Helper.CreateTapGestureRecognizer(TapGestureRecognizerButton_Tapped, GestureRecognizers);
            Helper.CreatePanGestureRecognizer(PanGesture_PanUpdated, GestureRecognizers);
        }

        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            ComboboxPanGesture(DropdownLabels, e.StatusType, ref currentPanY, 0, (DropdownLabels.Length * RowHeight) - ButtonHeight,
                e.TotalY, RowHeight, Invalidate);
        }

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

        public virtual void TapGestureRecognizerButton_Tapped(object? sender, TappedEventArgs e)
        {
            if (!ComboboxDropdownButtonTapped((int)Width - ButtonWidth, (int)Width, 0, ButtonHeight, ref buttonTapped,
                ref listboxVisible, ButtonHeight, ListboxHeight, (GraphicsView)this, Invalidate, e))
                ComboboxListboxLabelTapped(0, (float)Width, (float)Height, ref listboxVisible, DropdownLabels, RowHeight, ref currentPanY,
                    ref selectedItemIndex, ButtonHeight, ListboxHeight, (GraphicsView) this, Invalidate, e, SelectedItemChanged);
        }

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

        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRoundedRectangle(x, y, width, height, ButtonCornerRadius);
        }

        public virtual void DrawTextbox(ICanvas canvas, float x, float y, float width, float height)
        {
            if (SelectedItemIndex >= 0 && SelectedItemIndex < DropdownLabels.Length)
            {
                Helper.SetFontAttributes(canvas, TextboxFont);
                canvas.DrawString(DropdownLabels[SelectedItemIndex], x, y, width, height, TextboxFont.HorizontalAlignment, 
                    TextboxFont.VerticalAlignment);
            }
        }

        public virtual void DrawButton(ICanvas canvas, float x, float y, float width, float height) { }

        public virtual void DrawListboxFrame(ICanvas canvas, float x, float y, float width, float height) 
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(new Rect(x, y, width, height));
        }

        public virtual void DrawListboxLabel(ICanvas canvas, int row, float x, float y, float width, float height)
        {
            Helper.SetFontAttributes(canvas, LabelFont);
            canvas.DrawString(DropdownLabels[row], x, y, width, height, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
        }

        public virtual void ComboboxDropdownButtonTapped() { }
    }
}
