using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static MauiControlsLibrary.MCLListbox;

namespace MauiControlsLibrary
{
    public class MCLGrid : GraphicsView, IDrawable
    {
        public string[,]? Data { get; set; }
        public Microsoft.Maui.Graphics.Font DataFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color DataFontColor { get; set; } = Colors.Black;
        public int DataFontSize { get; set; } = 18;
        public HorizontalAlignment DataTextHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment DataTextVerticalAlignment { get; set; } = VerticalAlignment.Center;

        public string[]? HeaderNames { get; set; }
        public Microsoft.Maui.Graphics.Font HeaderFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color HeaderFontColor { get; set; } = Colors.Black;
        public int HeaderFontSize { get; set; } = 18;
        public HorizontalAlignment HeaderTextHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment HeaderTextVerticalAlignment { get; set; } = VerticalAlignment.Center;
        public int ColumnWidth { get; set; } = 100;
        public int DataRowHeight { get; set; } = 25;
        public int HeaderRowHeight { get; set; } = 50;

        private int currentPanY = 0;
        private int currentPanX = 0;

        public event EventHandler<GridEventArgs>? OnMCLGridTapped;

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
                    if(currentPanY < 0)
                        currentPanY = 0;
                    if(currentPanY > (Data.GetLength(0) - 1) * DataRowHeight + HeaderRowHeight)
                        currentPanY = (Data.GetLength(0) - 1) * DataRowHeight + HeaderRowHeight;
                    currentPanX += (int)e.TotalX;
                    if(currentPanX < 0)
                        currentPanX = 0;
                    if (currentPanX > (Data.GetLength(1) - 1) * ColumnWidth)
                        currentPanX = (Data.GetLength(1) - 1) * ColumnWidth;
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(panGesture);
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (Data != null && point.HasValue && point.Value.X >= 0 && point.Value.X <= this.Width
                    && point.Value.Y >= HeaderRowHeight && point.Value.Y < this.Height)
                {
                    int currentRowIndex = (int)Math.Floor(((decimal)currentPanY - HeaderRowHeight) / (decimal)DataRowHeight + (decimal)point.Value.Y / (decimal)DataRowHeight);
                    if (currentRowIndex < 0)
                        currentRowIndex = 0;
                    if (currentRowIndex >= Data.GetLength(0) - 1)
                        currentRowIndex = Data.GetLength(0) - 1;
                    int currentColIndex = (int)Math.Floor((decimal)currentPanX / (decimal)ColumnWidth + (decimal)point.Value.X / (decimal)ColumnWidth);
                    if (currentColIndex < 0)
                        currentColIndex = 0;
                    if(currentColIndex >= Data.GetLength(1)  - 1)
                        currentColIndex = Data.GetLength(1) - 1;
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
            if (HeaderNames != null)
            {
                for (int i = 0; i < HeaderNames.Length; i++)
                {
                    if (HeaderNames[i] != null)
                    {
                        Helper.SetFontAttributes(canvas, HeaderFont, HeaderFontColor, HeaderFontSize);
                        canvas.DrawString(HeaderNames[i], i * ColumnWidth, 0, ColumnWidth, HeaderRowHeight, HeaderTextHorizontalAlignment, HeaderTextVerticalAlignment);
                    }
                }
            }
            canvas.RestoreState();
            if (Data != null)
            {
                canvas.SaveState();
                canvas.ClipRectangle(0, HeaderRowHeight, (float)this.Width, (float)this.Height - HeaderRowHeight);
                int rowStart = (int)Math.Floor((decimal)currentPanY / (decimal)DataRowHeight);
                if(rowStart < 0)
                    rowStart = 0;
                if(rowStart > Data.GetLength(0) - 1)
                    rowStart = Data.GetLength(0) - 1;
                int colStart = (int)Math.Floor((decimal)currentPanX / (decimal)ColumnWidth);
                if(colStart < 0)
                    colStart = 0;
                if(colStart > Data.GetLength(1) - 1)
                    colStart = Data.GetLength(1) - 1;
                for(int row = rowStart; row < Data.GetLength(0) && row * DataRowHeight + HeaderRowHeight - currentPanY < this.Height; row++)
                {
                    for(int col=colStart; col < Data.GetLength(1) && col * ColumnWidth - currentPanX < this.Width; col++)
                    {
                        if (Data[row, col] != null)
                        {
                            Helper.SetFontAttributes(canvas, DataFont, DataFontColor, DataFontSize);
                            int rowOffset = row * DataRowHeight + HeaderRowHeight - currentPanY;
                            if(rowOffset < HeaderRowHeight && row == Data.GetLength(0) - 1)
                                rowOffset = HeaderRowHeight;
                            canvas.DrawString(Data[row, col], col * ColumnWidth - currentPanX, rowOffset, ColumnWidth,
                                DataRowHeight, DataTextHorizontalAlignment, DataTextVerticalAlignment);
                        }
                    }
                }
                canvas.RestoreState();
            }
        }
    }
}
