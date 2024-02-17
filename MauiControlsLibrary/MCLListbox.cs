namespace MauiControlsLibrary
{
    public class MCLListbox : GraphicsView, IDrawable
    {
        public string[] Labels { get; set; } = Array.Empty<string>();
        public Helper.StandardFontPropterties LabelFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);
        public Color? LabelBackgroundColor { get; set; } = null;
        public int RowHeight { get; set; } = 25;
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
            PanGestureRecognizer panGesture = new();
            panGesture.PanUpdated += PanGesture_PanUpdated;
            GestureRecognizers.Add(panGesture);
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(this);
            if (point.HasValue && Helper.PointFValueIsInRange(point, 0, Width, 0, Height))
            {
                int currentIndex = (int)Math.Floor((currentPanY / (decimal)RowHeight) + ((decimal)point.Value.Y / RowHeight));
                currentIndex = Helper.ValueResetOnBoundsCheck(currentIndex, 0, Labels.Length, moreThanMaxValueSet: Labels.Length - 1);
                OnMCLListboxTapped?.Invoke(this, new ListboxEventArgs(e, currentIndex));
                Invalidate();
            }
        }

        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            if (Labels != null && e.StatusType == GestureStatus.Running)
            {
                currentPanY += (int)e.TotalY;
                currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, (Labels.Length - 1) * RowHeight);
                Invalidate();
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
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

        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(new Rect(x, y, width, height));
        }

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
