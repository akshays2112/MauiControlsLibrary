namespace MauiControlsLibrary
{
    public abstract class MCLComboboxBase : GraphicsView, IDrawable
    {
        public Helper.StandardFontPropterties TextboxFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);
        public int ButtonWidth { get; set; } = 25;
        public int ButtonHeight { get; set; } = 25;
        public bool ButtonTapped { get; set; } = false;
        public int ButtonCornerRadius { get; set; } = 5;
        public bool ListboxVisible { get; set; } = false;
        public string[] DropdownLabels { get; set; } = Array.Empty<string>();
        public Helper.StandardFontPropterties LabelFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);
        public Color? LabelBackgroundColor { get; set; } = null;
        public int ListboxHeight { get; set; } = 200;
        public int RowHeight { get; set; } = 25;
        public int SelectedItemIndex { get; set; } = -1;
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
            TapGestureRecognizer tapGestureRecognizerButton = new();
            tapGestureRecognizerButton.Tapped += TapGestureRecognizerButton_Tapped;
            GestureRecognizers.Add(tapGestureRecognizerButton);
            PanGestureRecognizer panGesture = new();
            panGesture.PanUpdated += PanGesture_PanUpdated;
            GestureRecognizers.Add(panGesture);
        }

        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            if (Helper.ArrayNotNullOrEmpty(DropdownLabels) && e.StatusType == GestureStatus.Running)
            {
                currentPanY += (int)e.TotalY - RowHeight;
                currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, (DropdownLabels.Length * RowHeight) - ButtonHeight);
                Invalidate();
            }
        }

        public virtual void TapGestureRecognizerButton_Tapped(object? sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(this);
            if (Helper.PointFValueIsInRange(point, (int)Width - ButtonWidth, (int)Width, 0, ButtonHeight))
            {
                ButtonTapped = true;
                ListboxVisible = !ListboxVisible;
                HeightRequest = ListboxVisible ? ButtonHeight + ListboxHeight : ButtonHeight;
                Invalidate();
            }
            else if (Helper.ArrayNotNullOrEmpty(DropdownLabels) && point.HasValue && Helper.IntValueIsInRange((int)point.Value.X, 0, (int)Width)
                && Helper.IntValueIsInRange((int)point.Value.Y, RowHeight, (int)Height))
            {
                int currentRowIndex = (int)Math.Floor(((currentPanY - RowHeight) / (decimal)RowHeight) + ((decimal)point.Value.Y / RowHeight));
                currentRowIndex = Helper.ValueResetOnBoundsCheck(currentRowIndex, 0, DropdownLabels.Length - 1);
                SelectedItemIndex = currentRowIndex;
                ListboxVisible = !ListboxVisible;
                HeightRequest = ListboxVisible ? ButtonHeight + ListboxHeight : ButtonHeight;
                SelectedItemChanged?.Invoke(this, new ComboboxEventArgs(e, currentRowIndex));
                Invalidate();
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
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
    }
}
