namespace MauiControlsLibrary
{
    public class MCLGrid : GraphicsView, IDrawable
    {
        public string[,]? Data { get; set; }
        public Helper.StandardFontPropterties DataFont { get; set; } = new();

        public string[]? HeaderNames { get; set; }
        public Helper.StandardFontPropterties HeaderFont { get; set; } = new();
        public int ColumnWidth { get; set; } = 100;
        public int DataRowHeight { get; set; } = 25;
        public int HeaderRowHeight { get; set; } = 50;

        public event EventHandler<GridEventArgs>? OnMCLGridTapped;

        private int currentPanY = 0;
        private int currentPanX = 0;

        public class GridEventArgs
        {
            public EventArgs? EventArgs { get; set; }
            public int CurrentRowIndex { get; set; }
            public int CurrentColIndex { get; set; }

            public GridEventArgs(EventArgs? eventArgs, int currentRowIndex, int currentColIndex)
            {
                EventArgs = eventArgs;
                CurrentRowIndex = currentRowIndex;
                CurrentColIndex = currentColIndex;
            }
        }

        public MCLGrid()
        {
            this.Drawable = this;
            PanGestureRecognizer panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (s, e) => {
                if (Data != null && e.StatusType == GestureStatus.Running)
                {
                    currentPanY += (int)e.TotalY;
                    currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, (Data.GetLength(0) - 1) * DataRowHeight + HeaderRowHeight);
                    currentPanX += (int)e.TotalX;
                    currentPanX = Helper.ValueResetOnBoundsCheck(currentPanX, 0, (Data.GetLength(1) - 1) * ColumnWidth);
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(panGesture);
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Data != null && Data.Length > 0 && point.HasValue && Helper.PointFValueIsInRange(point, 0, this.Width, HeaderRowHeight, this.Height))
                {
                    int currentRowIndex = (int)Math.Floor(((decimal)currentPanY - HeaderRowHeight) / (decimal)DataRowHeight + 
                        (decimal)point.Value.Y / (decimal)DataRowHeight);
                    currentRowIndex = Helper.ValueResetOnBoundsCheck(currentRowIndex, 0, Data.GetLength(0) - 1);
                    int currentColIndex = (int)Math.Floor((decimal)currentPanX / (decimal)ColumnWidth + (decimal)point.Value.X / (decimal)ColumnWidth);
                    currentColIndex = Helper.ValueResetOnBoundsCheck(currentColIndex, 0, Data.GetLength(1) - 1);
                    if (OnMCLGridTapped != null)
                        OnMCLGridTapped(this, new GridEventArgs(e, currentRowIndex, currentColIndex));
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(0, 0, (float)this.Width, (float)this.Height);
            canvas.SaveState();
            canvas.ClipRectangle(0, 0, (float)this.Width, (float)this.Height);
            if (HeaderNames != null && HeaderNames.Length > 0)
            {
                int colStart = (int)Math.Floor((decimal)currentPanX / (decimal)ColumnWidth);
                colStart = Helper.ValueResetOnBoundsCheck(colStart, 0, HeaderNames.Length - 1);
                for (int col = colStart; col < HeaderNames.Length; col++)
                {
                    if (HeaderNames[col] != null)
                    {
                        Helper.SetFontAttributes(canvas, HeaderFont);
                        canvas.DrawString(HeaderNames[col], col * ColumnWidth - currentPanX, 0, ColumnWidth, HeaderRowHeight,
                            HeaderFont.HorizontalAlignment, HeaderFont.VerticalAlignment);
                    }
                }
            }
            canvas.RestoreState();
            if (Data != null && Data.Length > 0)
            {
                canvas.SaveState();
                canvas.ClipRectangle(0, HeaderRowHeight, (float)this.Width, (float)this.Height - HeaderRowHeight);
                int rowStart = (int)Math.Floor((decimal)currentPanY / (decimal)DataRowHeight);
                rowStart = Helper.ValueResetOnBoundsCheck(rowStart, 0, Data.GetLength(0) - 1);
                int colStart = (int)Math.Floor((decimal)currentPanX / (decimal)ColumnWidth);
                colStart = Helper.ValueResetOnBoundsCheck(colStart, 0, Data.GetLength(1) - 1);
                for(int row = rowStart; row < Data.GetLength(0) && row * DataRowHeight + HeaderRowHeight - currentPanY < this.Height; row++)
                {
                    for(int col=colStart; col < Data.GetLength(1) && col * ColumnWidth - currentPanX < this.Width; col++)
                    {
                        if (Data[row, col] != null)
                        {
                            Helper.SetFontAttributes(canvas, DataFont);
                            int rowOffset = row * DataRowHeight + HeaderRowHeight - currentPanY;
                            if(rowOffset < HeaderRowHeight && row == Data.GetLength(0) - 1)
                                rowOffset = HeaderRowHeight;
                            canvas.DrawString(Data[row, col], col * ColumnWidth - currentPanX, rowOffset, ColumnWidth,
                                DataRowHeight, DataFont.HorizontalAlignment, DataFont.VerticalAlignment);
                        }
                    }
                }
                canvas.RestoreState();
            }
        }
    }
}
