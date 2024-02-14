using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiControlsLibrary
{
    public class MCLListbox : GraphicsView, IDrawable
    {
        public string[] Labels { get; set; } = Array.Empty<string>();
        public Microsoft.Maui.Graphics.Font LabelFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color LabelFontColor { get; set; } = Colors.Black;
        public int LabelFontSize { get; set; } = 18;
        public HorizontalAlignment LabelTextHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment LabelTextVerticalAlignment { get; set; } = VerticalAlignment.Top;
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
                    if (currentPanY < 0)
                        currentPanY = 0;
                    if (currentPanY > (Labels.Length - 1) * RowHeight)
                        currentPanY = (Labels.Length - 1) * RowHeight;
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(panGesture);
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (point.HasValue && point.Value.X >= 0 && point.Value.X <= this.Width
                    && point.Value.Y >= 0 && point.Value.Y < this.Height)
                {
                    int currentIndex = (int)Math.Floor((decimal)currentPanY / (decimal)RowHeight + (decimal)point.Value.Y / (decimal)RowHeight);
                    if (currentIndex < 0)
                        currentIndex = 0;
                    if (currentIndex >= Labels.Length)
                        currentIndex = Labels.Length - 1;
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
            if (Labels != null)
            {
                canvas.SaveState();
                canvas.ClipRectangle(0, 0, (float)this.Width, (float)this.Height);
                int rowStart = (int)Math.Floor((decimal)currentPanY / (decimal)RowHeight);
                if (rowStart < 0)
                    rowStart = 0;
                if (rowStart > Labels.Length - 1)
                    rowStart = Labels.Length - 1;
                for (int row = rowStart; row < Labels.Length && row * RowHeight - currentPanY < this.Height; row++)
                {
                    if (Labels[row] != null)
                    {
                        Helper.SetFontAttributes(canvas, LabelFont, LabelFontColor, LabelFontSize);
                        canvas.DrawString(Labels[row], 0, row * RowHeight - currentPanY, (float)this.Width,
                            RowHeight, LabelTextHorizontalAlignment, LabelTextVerticalAlignment);
                    }
                }
            }
            canvas.ResetState();
        }
    }
}
