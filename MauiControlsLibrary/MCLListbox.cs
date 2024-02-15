namespace MauiControlsLibrary
{
    public class MCLListbox : GraphicsView, IDrawable
    {
        public string[] Labels { get; set; } = Array.Empty<string>();
        public Helper.StandardFontPropterties LabelFont { get; set; } = new(verticalAlignment: VerticalAlignment.Top);
        public Color? LabelBackgroundColor { get; set; } = null;
        public int RowHeight { get; set; } = 25;
        public event EventHandler<ListboxEventArgs>? OnMCLListboxTapped;

        public class ListboxEventArgs
        {
            public EventArgs? EventArgs { get; set; }
            public int CurrentIndex { get; set; }

            public ListboxEventArgs(EventArgs? eventArgs, int currentIndex)
            {
                EventArgs = eventArgs;
                CurrentIndex = currentIndex;
            }
        }

        private int currentPanY = 0;

        public MCLListbox()
        {
            this.Drawable = this;
            PanGestureRecognizer panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (s, e) =>
            {
                if (Labels != null && e.StatusType == GestureStatus.Running)
                {
                    currentPanY += (int)e.TotalY;
                    currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, (Labels.Length - 1) * RowHeight);
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(panGesture);
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (point.HasValue && Helper.PointFValueIsInRange(point, 0, this.Width, 0, this.Height))
                {
                    int currentIndex = (int)Math.Floor((decimal)currentPanY / (decimal)RowHeight + (decimal)point.Value.Y / (decimal)RowHeight);
                    currentIndex = Helper.ValueResetOnBoundsCheck(currentIndex, 0, Labels.Length, moreThanMaxValueSet: Labels.Length - 1);
                    if (OnMCLListboxTapped != null)
                        OnMCLListboxTapped(this, new ListboxEventArgs(e, currentIndex));
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(new Rect(0, 0, this.Width, this.Height));
            canvas.SaveState();
            canvas.ClipRectangle(0, 0, (float)this.Width, (float)this.Height);
            if (Labels != null && Labels.Length > 0)
            {
                canvas.SaveState();
                canvas.ClipRectangle(0, 0, (float)this.Width, (float)this.Height);
                int rowStart = (int)Math.Floor((decimal)currentPanY / (decimal)RowHeight);
                rowStart = Helper.ValueResetOnBoundsCheck(rowStart, 0, Labels.Length - 1);
                for (int row = rowStart; row < Labels.Length && row * RowHeight - currentPanY < this.Height; row++)
                {
                    if (Labels[row] != null)
                    {
                        Helper.SetFontAttributes(canvas, LabelFont);
                        canvas.DrawString(Labels[row], 0, row * RowHeight - currentPanY, (float)this.Width,
                            RowHeight, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
                    }
                }
            }
            canvas.ResetState();
        }
    }
}
