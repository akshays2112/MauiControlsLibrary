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
            Helper.CreatePanGestureRecognizer(PanGesture_PanUpdated, GestureRecognizers);
            Helper.CreateTapGestureRecognizer(TapGestureRecognizer_Tapped, GestureRecognizers);
        }

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            ListboxTapped(0, (float)Width, 0, (float)Height, currentPanY, RowHeight, Labels, OnMCLListboxTapped, this, e, Invalidate);
        }

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

        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            ListboxPan(Labels, ref currentPanY, RowHeight, e, Invalidate);
        }

        public static void ListboxPan(string[] labels, ref int currentPanY, int rowHeight, PanUpdatedEventArgs e, Action invalidate)
        {
            if (labels != null && e.StatusType == GestureStatus.Running)
            {
                currentPanY += (int)e.TotalY;
                currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, (labels.Length - 1) * rowHeight);
                invalidate();
            }
        }

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
