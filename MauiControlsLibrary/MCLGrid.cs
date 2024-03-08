namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a grid control.
    /// </summary>
    public class MCLGrid : GraphicsView, IDrawable
    {
        /// <summary>
        /// Gets or sets the data to be displayed in the grid.
        /// </summary>
        public string[,]? Data { get; set; }

        /// <summary>
        /// Gets or sets the font properties for the data cells.
        /// </summary>
        public Helper.StandardFontPropterties DataFont { get; set; } = new();

        /// <summary>
        /// Gets or sets the header names for the grid.
        /// </summary>
        public string[]? HeaderNames { get; set; }

        /// <summary>
        /// Gets or sets the font properties for the header cells.
        /// </summary>
        public Helper.StandardFontPropterties HeaderFont { get; set; } = new();

        /// <summary>
        /// Gets or sets the width of the columns.
        /// </summary>
        public int ColumnWidth { get; set; } = 100;

        /// <summary>
        /// Gets or sets the height of the data rows.
        /// </summary>
        public int DataRowHeight { get; set; } = 25;

        /// <summary>
        /// Gets or sets the height of the header row.
        /// </summary>
        public int HeaderRowHeight { get; set; } = 50;

        /// <summary>
        /// Event triggered when the grid is tapped.
        /// </summary>
        public event EventHandler<GridEventArgs>? OnMCLGridTapped;

        private int currentPanY = 0;
        private int currentPanX = 0;

        /// <summary>
        /// Represents the arguments for the grid event.
        /// </summary>
        public class GridEventArgs(EventArgs? eventArgs, int currentRowIndex, int currentColIndex) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public int CurrentRowIndex { get; set; } = currentRowIndex;
            public int CurrentColIndex { get; set; } = currentColIndex;
        }

        /// <summary>
        /// Initializes a new instance of the MCLGrid class.
        /// </summary>
        public MCLGrid()
        {
            Drawable = this;
            Helper.CreateTapGestureRecognizer(TapGestureRecognizer_Tapped, GestureRecognizers);
            Helper.CreatePanGestureRecognizer(PanGesture_PanUpdated, GestureRecognizers);
        }

        /// <summary>
        /// Handles the tap gesture on the grid.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            GridTapped(0, (float)Width, HeaderRowHeight, (float)Height, currentPanY, HeaderRowHeight, DataRowHeight, Data, currentPanX, ColumnWidth,
                (GraphicsView)this, e, OnMCLGridTapped, Invalidate);
        }

        /// <summary>
        /// Handles the grid tap event.
        /// </summary>
        /// <param name="x">The x-coordinate of the tap.</param>
        /// <param name="width">The width of the grid.</param>
        /// <param name="y">The y-coordinate of the tap.</param>
        /// <param name="height">The height of the grid.</param>
        /// <param name="currentPanY">The current vertical pan position.</param>
        /// <param name="headerRowHeight">The height of the header row.</param>
        /// <param name="dataRowHeight">The height of the data rows.</param>
        /// <param name="data">The data in the grid.</param>
        /// <param name="currentPanX">The current horizontal pan position.</param>
        /// <param name="columnWidth">The width of the columns.</param>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        /// <param name="onMCLGridTapped">The event handler for the grid tap event.</param>
        /// <param name="invalidate">The action to invalidate the grid.</param>
        public static void GridTapped(float x, float width, float y, float height, int currentPanY, int headerRowHeight, int dataRowHeight,
            string[,]? data, int currentPanX, int columnWidth, GraphicsView sender, TappedEventArgs e, EventHandler<GridEventArgs>? onMCLGridTapped,
            Action invalidate)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.ArrayNotNullOrEmpty(data) && point.HasValue && Helper.PointFValueIsInRange(point, x, width, y, height))
            {
                int currentRowIndex = (int)Math.Floor((((decimal)currentPanY - headerRowHeight) / dataRowHeight) +
                    ((decimal)point.Value.Y / dataRowHeight));
                currentRowIndex = Helper.ValueResetOnBoundsCheck(currentRowIndex, 0, data?.GetLength(0) - 1 ?? 0);
                int currentColIndex = (int)Math.Floor((currentPanX / (decimal)columnWidth) + ((decimal)point.Value.X / columnWidth));
                currentColIndex = Helper.ValueResetOnBoundsCheck(currentColIndex, 0, data?.GetLength(1) - 1 ?? 0);
                onMCLGridTapped?.Invoke(sender, new GridEventArgs(e, currentRowIndex, currentColIndex));
                invalidate();
            }
        }

        /// <summary>
        /// Handles the pan gesture update on the grid.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            GridPan(Data, ref currentPanY, ref currentPanX, DataRowHeight, HeaderRowHeight, ColumnWidth, e, Invalidate);
        }

        /// <summary>
        /// Handles the grid pan event.
        /// </summary>
        /// <param name="data">The data in the grid.</param>
        /// <param name="currentPanY">The current vertical pan position.</param>
        /// <param name="currentPanX">The current horizontal pan position.</param>
        /// <param name="dataRowHeight">The height of the data rows.</param>
        /// <param name="headerRowHeight">The height of the header row.</param>
        /// <param name="columnWidth">The width of the columns.</param>
        /// <param name="e">The event arguments.</param>
        /// <param name="invalidate">The action to invalidate the grid.</param>
        public static void GridPan(string[,]? data, ref int currentPanY, ref int currentPanX, int dataRowHeight, int headerRowHeight, int columnWidth,
            PanUpdatedEventArgs e, Action invalidate)
        {
            if (data != null && e.StatusType == GestureStatus.Running)
            {
                currentPanY += (int)e.TotalY;
                currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, ((data.GetLength(0) - 1) * dataRowHeight) + headerRowHeight);
                currentPanX += (int)e.TotalX;
                currentPanX = Helper.ValueResetOnBoundsCheck(currentPanX, 0, (data.GetLength(1) - 1) * columnWidth);
                invalidate();
            }
        }

        /// <summary>
        /// Draws the grid on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area of the canvas that needs to be redrawn.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Draw the frame of the grid
            DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            canvas.SaveState();
            canvas.ClipRectangle(0, 0, (float)Width, (float)Height);

            // Draw the headers of the grid
            if (Helper.ArrayNotNullOrEmpty(HeaderNames))
            {
                int colStart = (int)Math.Floor(currentPanX / (decimal)ColumnWidth);
                colStart = Helper.ValueResetOnBoundsCheck(colStart, 0, HeaderNames?.Length - 1 ?? 0);
                for (int col = colStart; col < HeaderNames?.Length; col++)
                {
                    if (HeaderNames[col] != null)
                    {
                        DrawHeaderName(canvas, col, HeaderNames[col], (col * ColumnWidth) - currentPanX, 0, ColumnWidth, HeaderRowHeight);
                    }
                }
            }
            _ = canvas.RestoreState();

            // Draw the data of the grid
            if (Helper.ArrayNotNullOrEmpty(Data))
            {
                canvas.SaveState();
                canvas.ClipRectangle(0, HeaderRowHeight, (float)Width, (float)Height - HeaderRowHeight);
                int rowStart = (int)Math.Floor(currentPanY / (decimal)DataRowHeight);
                rowStart = Helper.ValueResetOnBoundsCheck(rowStart, 0, Data?.GetLength(0) - 1 ?? 0);
                int colStart = (int)Math.Floor(currentPanX / (decimal)ColumnWidth);
                colStart = Helper.ValueResetOnBoundsCheck(colStart, 0, Data?.GetLength(1) - 1 ?? 0);
                for (int row = rowStart; row < Data?.GetLength(0) && (row * DataRowHeight) + HeaderRowHeight - currentPanY < Height; row++)
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

        /// <summary>
        /// Draws the frame of the grid.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the frame.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the frame.</param>
        /// <param name="width">The width of the frame.</param>
        /// <param name="height">The height of the frame.</param>
        protected virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(x, y, (float)Width, (float)Height);
        }

        /// <summary>
        /// Draws a header name in the grid.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="headerColumnIndex">The index of the header column.</param>
        /// <param name="headerLabel">The label of the header.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the header cell.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the header cell.</param>
        /// <param name="width">The width of the header cell.</param>
        /// <param name="height">The height of the header cell.</param>
        protected virtual void DrawHeaderName(ICanvas canvas, int headerColumnIndex, string headerLabel, float x, float y, float width, float height)
        {
            Helper.SetFontAttributes(canvas, HeaderFont);
            canvas.DrawString(headerLabel, x, y, width, height, HeaderFont.HorizontalAlignment, HeaderFont.VerticalAlignment);
        }

        /// <summary>
        /// Draws a cell label in the grid.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="cellRow">The row index of the cell.</param>
        /// <param name="cellCol">The column index of the cell.</param>
        /// <param name="cellLabel">The label of the cell.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the cell.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the cell.</param>
        /// <param name="width">The width of the cell.</param>
        /// <param name="height">The height of the cell.</param>
        protected virtual void DrawGridCellLabel(ICanvas canvas, int cellRow, int cellCol, string cellLabel, float x, float y, float width, float height)
        {
            Helper.SetFontAttributes(canvas, DataFont);
            canvas.DrawString(cellLabel, x, y, width, height, DataFont.HorizontalAlignment, DataFont.VerticalAlignment);
        }
    }
}
