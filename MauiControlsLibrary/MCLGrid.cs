using Xamarin.Google.Crypto.Tink.Signature;

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

        public class GridEventArgs(EventArgs? eventArgs, int currentRowIndex, int currentColIndex) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public int CurrentRowIndex { get; set; } = currentRowIndex;
            public int CurrentColIndex { get; set; } = currentColIndex;
        }

        public MCLGrid()
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
            if (Helper.ArrayNotNullOrEmpty(Data) && point.HasValue && Helper.PointFValueIsInRange(point, 0, Width, HeaderRowHeight, Height))
            {
                int currentRowIndex = (int)Math.Floor((((decimal)currentPanY - HeaderRowHeight) / DataRowHeight) +
                    ((decimal)point.Value.Y / DataRowHeight));
                currentRowIndex = Helper.ValueResetOnBoundsCheck(currentRowIndex, 0, Data.GetLength(0) - 1);
                int currentColIndex = (int)Math.Floor((currentPanX / (decimal)ColumnWidth) + ((decimal)point.Value.X / ColumnWidth));
                currentColIndex = Helper.ValueResetOnBoundsCheck(currentColIndex, 0, Data.GetLength(1) - 1);
                OnMCLGridTapped?.Invoke(this, new GridEventArgs(e, currentRowIndex, currentColIndex));
                Invalidate();
            }
        }

        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            if (Data != null && e.StatusType == GestureStatus.Running)
            {
                currentPanY += (int)e.TotalY;
                currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, ((Data.GetLength(0) - 1) * DataRowHeight) + HeaderRowHeight);
                currentPanX += (int)e.TotalX;
                currentPanX = Helper.ValueResetOnBoundsCheck(currentPanX, 0, (Data.GetLength(1) - 1) * ColumnWidth);
                Invalidate();
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            canvas.SaveState();
            canvas.ClipRectangle(0, 0, (float)Width, (float)Height);
            if (Helper.ArrayNotNullOrEmpty(HeaderNames))
            {
                int colStart = (int)Math.Floor(currentPanX / (decimal)ColumnWidth);
                colStart = Helper.ValueResetOnBoundsCheck(colStart, 0, HeaderNames.Length - 1);
                for (int col = colStart; col < HeaderNames.Length; col++)
                {
                    if (HeaderNames[col] != null)
                    {
                        DrawHeaderName(canvas, col, HeaderNames[col], (col * ColumnWidth) - currentPanX, 0, ColumnWidth, HeaderRowHeight);
                    }
                }
            }
            _ = canvas.RestoreState();
            if (Helper.ArrayNotNullOrEmpty(Data))
            {
                canvas.SaveState();
                canvas.ClipRectangle(0, HeaderRowHeight, (float)Width, (float)Height - HeaderRowHeight);
                int rowStart = (int)Math.Floor(currentPanY / (decimal)DataRowHeight);
                rowStart = Helper.ValueResetOnBoundsCheck(rowStart, 0, Data.GetLength(0) - 1);
                int colStart = (int)Math.Floor(currentPanX / (decimal)ColumnWidth);
                colStart = Helper.ValueResetOnBoundsCheck(colStart, 0, Data.GetLength(1) - 1);
                for (int row = rowStart; row < Data.GetLength(0) && (row * DataRowHeight) + HeaderRowHeight - currentPanY < Height; row++)
                {
                    for (int col = colStart; col < Data.GetLength(1) && (col * ColumnWidth) - currentPanX < Width; col++)
                    {
                        if (Data[row, col] != null)
                        {
                            int rowOffset = (row * DataRowHeight) + HeaderRowHeight - currentPanY;
                            if (rowOffset < HeaderRowHeight && row == Data.GetLength(0) - 1)
                            {
                                rowOffset = HeaderRowHeight;
                            }
                            DrawGridCellLabel(canvas, row, col, Data[row, col], (col * ColumnWidth) - currentPanX, rowOffset, ColumnWidth,
                                DataRowHeight);
                        }
                    }
                }
                _ = canvas.RestoreState();
            }
        }

        protected virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(x, y, (float)Width, (float)Height);
        }

        protected virtual void DrawHeaderName(ICanvas canvas, int headerColumnIndex, string headerLabel, float x, float y, float width, float height)
        {
            Helper.SetFontAttributes(canvas, HeaderFont);
            canvas.DrawString(headerLabel, x, y, width, height, HeaderFont.HorizontalAlignment, HeaderFont.VerticalAlignment);
        }

        protected virtual void DrawGridCellLabel(ICanvas canvas, int cellRow, int cellCol, string cellLabel, float x, float y, float width, float height)
        {
            Helper.SetFontAttributes(canvas, DataFont);
            canvas.DrawString(cellLabel, x, y, width, height, DataFont.HorizontalAlignment, DataFont.VerticalAlignment);
        }
    }
}
