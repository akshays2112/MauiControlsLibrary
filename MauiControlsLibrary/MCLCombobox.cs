namespace MauiControlsLibrary
{
    public class MCLCombobox : GraphicsView, IDrawable
    {
        public Helper.StandardFontPropterties TextboxFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);

        public string ButtonTextForListboxCollapsed { get; set; } = "▼";
        public string ButtonTextForListboxExpanded { get; set; } = "▲";
        public Helper.StandardFontPropterties ButtonFont { get; set; } = new();
        public Color ButtonColor { get; set; } = Colors.Green;
        public Color ButtonTappedColor { get; set; } = Colors.Red;
        public int ButtonWidth { get; set; } = 25;
        public int ButtonHeight { get; set; } = 25;
        public int ButtonCornerRadius { get; set; } = 5;
        public bool ButtonTapped { get; set; } = false;
        public bool ListboxVisible { get; set; } = false;

        public string[] Labels { get; set; } = Array.Empty<string>();
        public Helper.StandardFontPropterties LabelFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);
        public Color? LabelBackgroundColor { get; set; } = null;
        public int ListboxHeight { get; set; } = 200;
        public int RowHeight { get; set; } = 25;

        public int SelectedItemIndex { get; set; } = -1;
        public event EventHandler<ComboboxEventArgs>? SelectedItemChanged;
        private int currentPanY = 0;

        public class ComboboxEventArgs(EventArgs? eventArgs, int selectedIndex) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public int SelectedIndex { get; set; } = selectedIndex;
        }

        public MCLCombobox()
        {
            Drawable = this;
            TapGestureRecognizer tapGestureRecognizerButton = new();
            tapGestureRecognizerButton.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Helper.PointFValueIsInRange(point, (int)Width - ButtonWidth, (int)Width, 0, ButtonHeight))
                {
                    ButtonTapped = true;
                    ListboxVisible = !ListboxVisible;
                    HeightRequest = ListboxVisible ? ButtonHeight + ListboxHeight : ButtonHeight;
                    Invalidate();
                }
            };
            GestureRecognizers.Add(tapGestureRecognizerButton);
            PanGestureRecognizer panGesture = new();
            panGesture.PanUpdated += (s, e) =>
            {
                if (Helper.ArrayNotNullOrEmpty(Labels) && e.StatusType == GestureStatus.Running)
                {
                    currentPanY += (int)e.TotalY;
                    currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, ((Labels.Length - 1) * RowHeight) - ButtonHeight);
                    Invalidate();
                }
            };
            GestureRecognizers.Add(panGesture);
            TapGestureRecognizer tapGestureRecognizerListbox = new();
            tapGestureRecognizerListbox.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Helper.ArrayNotNullOrEmpty(Labels) && point.HasValue && Helper.IntValueIsInRange((int)point.Value.X, 0, (int)Width)
                    && Helper.IntValueIsInRange((int)point.Value.Y, RowHeight, (int)Height))
                {
                    int currentRowIndex = (int)Math.Floor((currentPanY / (decimal)RowHeight) + ((decimal)point.Value.Y / RowHeight));
                    currentRowIndex = Helper.ValueResetOnBoundsCheck(currentRowIndex, 0, Labels.Length - 1);
                    SelectedItemIndex = currentRowIndex;
                    ListboxVisible = !ListboxVisible;
                    HeightRequest = ListboxVisible ? ButtonHeight + ListboxHeight : ButtonHeight;
                    SelectedItemChanged?.Invoke(this, new ComboboxEventArgs(e, currentRowIndex));
                    Invalidate();
                }
            };
            GestureRecognizers.Add(tapGestureRecognizerListbox);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRoundedRectangle(0, 0, (float)Width, ButtonHeight, ButtonCornerRadius);
            if (SelectedItemIndex >= 0 && SelectedItemIndex < Labels.Length)
            {
                Helper.SetFontAttributes(canvas, TextboxFont);
                canvas.DrawString(Labels[SelectedItemIndex], 0, 0, (float)Width - ButtonHeight, ButtonHeight,
                    TextboxFont.HorizontalAlignment, TextboxFont.VerticalAlignment);
            }
            if (ButtonTapped)
            {
                ButtonTapped = false;
                DrawButton(canvas, ButtonTappedColor);
                Invalidate();
            }
            else
            {
                DrawButton(canvas, ButtonColor);
            }
            if (ListboxVisible && Helper.ArrayNotNullOrEmpty(Labels))
            {
                canvas.StrokeColor = Colors.Grey;
                canvas.DrawRectangle(new Rect(0, ButtonHeight, Width, Height - ButtonHeight));
                canvas.SaveState();
                canvas.ClipRectangle(0, ButtonHeight, (float)Width, (float)Height - ButtonHeight);
                int rowStart = (int)Math.Floor(currentPanY / (decimal)RowHeight);
                rowStart = Helper.ValueResetOnBoundsCheck(rowStart, 0, Labels.Length - 1);
                for (int row = rowStart; row < Labels.Length && (row * RowHeight) - currentPanY < Height; row++)
                {
                    if (Labels[row] != null)
                    {
                        Helper.SetFontAttributes(canvas, LabelFont);
                        canvas.DrawString(Labels[row], 0, (row * RowHeight) - currentPanY, (float)Width,
                            RowHeight, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
                    }
                }
                canvas.ResetState();
            }
        }

        private void DrawButton(ICanvas canvas, Color color)
        {
            canvas.FillColor = color;
            canvas.FillRoundedRectangle(new RectF((float)Width - ButtonHeight, 0, ButtonHeight, ButtonHeight), ButtonCornerRadius);
            if (!string.IsNullOrEmpty(ButtonTextForListboxCollapsed))
            {
                Helper.SetFontAttributes(canvas, ButtonFont);
                canvas.DrawString(ListboxVisible ? ButtonTextForListboxExpanded : ButtonTextForListboxCollapsed, (float)Width - ButtonHeight,
                    0, ButtonHeight, ButtonHeight, ButtonFont.HorizontalAlignment, ButtonFont.VerticalAlignment);
            }
        }
    }
}
