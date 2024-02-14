using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiControlsLibrary
{
    public class MCCLListbox : GraphicsView, IDrawable
    {
        public string[] Labels { get; set; } = Array.Empty<string>();
        public Microsoft.Maui.Graphics.Font LabelTextFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color LabelTextColor { get; set; } = Colors.Black;
        public int LabelTextFontSize { get; set; } = 18;
        public HorizontalAlignment LabelTextHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment LabelTextVerticalAlignment { get; set; } = VerticalAlignment.Top;
        public Color? LabelBackgroundColor { get; set; } = null;
        public int RowHeight { get; set; } = 25;
        public event EventHandler<ListboxEventArgs>? OnMCCLListboxTapped;

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

        public MCCLListbox()
        {
            this.Drawable = this;
            PanGestureRecognizer panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (s, e) => {
                if (e.StatusType == GestureStatus.Running)
                {
                    currentPanY += (int)e.TotalY;
                    if(currentPanY < 0)
                        currentPanY = 0;
                    if(currentPanY > (Labels.Length - 1) * RowHeight)
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
                    if (OnMCCLListboxTapped != null)
                        OnMCCLListboxTapped(this, new ListboxEventArgs(e, currentIndex));
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
            int totalHeight = Labels.Length * RowHeight;
            if(currentPanY > totalHeight)
                currentPanY = totalHeight - RowHeight;
            int currentIndex = (int)Math.Floor((decimal)currentPanY / (decimal)RowHeight);
            if (currentIndex < 0)
                currentIndex = 0;
            if (currentIndex >= Labels.Length)
                currentIndex = Labels.Length - 1;
            int yOffset = currentIndex * RowHeight - currentPanY;
            if(yOffset > RowHeight) yOffset = 0;
            if(yOffset < -RowHeight) yOffset = -RowHeight;
            for (int i = currentIndex; i < Labels.Length && (i - currentIndex) * RowHeight + yOffset < this.Height; i++)
            {
                if (LabelBackgroundColor != null)
                {
                    canvas.FillColor = LabelBackgroundColor;
                    canvas.FillRectangle(new Rect(0, 0, this.Width, this.Height));
                }
                canvas.Font = LabelTextFont;
                canvas.FontColor = LabelTextColor;
                canvas.FontSize = LabelTextFontSize;
                canvas.DrawString(Labels[i], 0, (i - currentIndex) * RowHeight + yOffset, (float)this.Width, (float)this.Height, LabelTextHorizontalAlignment, LabelTextVerticalAlignment);
            }
            canvas.ResetState();
        }
    }
}
