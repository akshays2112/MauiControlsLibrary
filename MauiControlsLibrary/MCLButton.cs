using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLButton : GraphicsView, IDrawable
    {
        public string ButtonText { get; set; } = string.Empty;
        public Microsoft.Maui.Graphics.Font ButtonTextFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color ButtonTextColor { get; set; } = Colors.Black;
        public int ButtonTextFontSize { get; set; } = 18;
        public HorizontalAlignment ButtonTextHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment ButtonTextVerticalAlignment { get; set; } = VerticalAlignment.Center;
        public Color ButtonColor { get; set; } = Colors.Green;
        public Color ButtonTappedColor { get; set; } = Colors.Red;
        public double ButtonCornerRadius { get; set; } = 5;
        public Microsoft.Maui.Graphics.IImage? ButtonBackgroundNotPressedImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? ButtonBackgroundPressedImage { get; set; }
        public bool ClipButtonBackgroundImage { get; set; } = true;
        public event EventHandler<EventArgs>? OnMCLButtonTapped;
        public bool Tapped { get; set; } = false;

        public MCLButton()
        {
            this.Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if(point.HasValue && point.Value.X >= 0 && point.Value.X <= this.Width
                    && point.Value.Y >= 0 && point.Value.Y < this.Height)
                {
                    Tapped = true;
                    if (OnMCLButtonTapped != null)
                        OnMCLButtonTapped(this, e);
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Tapped)
            {
                Tapped = false;
                if (ButtonBackgroundPressedImage != null)
                {
                    canvas.SaveState();
                    canvas.ClipRectangle(0, 0, (float)this.Width, (float)this.Height);
                    if (ClipButtonBackgroundImage)
                    {
                        canvas.DrawImage(ButtonBackgroundPressedImage, 0, 0, ButtonBackgroundPressedImage.Width < this.Width ? (float)this.Width :
                            ButtonBackgroundPressedImage.Width, ButtonBackgroundPressedImage.Height < this.Height ? (float)this.Height :
                            ButtonBackgroundPressedImage.Height);
                    }
                    else
                    {
                        canvas.DrawImage(ButtonBackgroundPressedImage, 0, 0, (float)this.Width, (float)this.Height);
                    }
                    canvas.ResetState();
                }
                else
                {
                    DrawButton(canvas, this.ButtonTappedColor);
                }
            }
            else
            {
                if (ButtonBackgroundNotPressedImage != null)
                {
                    canvas.SaveState();
                    canvas.ClipRectangle(0, 0, (float)this.Width, (float)this.Height);
                    if (ClipButtonBackgroundImage)
                    {
                        canvas.DrawImage(ButtonBackgroundNotPressedImage, 0, 0, ButtonBackgroundNotPressedImage.Width < this.Width ? (float)this.Width :
                            ButtonBackgroundNotPressedImage.Width, ButtonBackgroundNotPressedImage.Height < this.Height ? (float)this.Height :
                            ButtonBackgroundNotPressedImage.Height);
                    }
                    else
                    {
                        canvas.DrawImage(ButtonBackgroundNotPressedImage, 0, 0, (float)this.Width, (float)this.Height);
                    }
                    canvas.ResetState();
                }
                else
                {
                    DrawButton(canvas, this.ButtonColor);
                }
            }
            this.Invalidate();
        }

        private void DrawButton(ICanvas canvas, Color color)
        {
            canvas.FillColor = color;
            canvas.FillRoundedRectangle(new Rect(0, 0, this.Width, this.Height), ButtonCornerRadius);
            if (!string.IsNullOrEmpty(ButtonText))
            {
                Helper.SetFontAttributes(canvas, ButtonTextFont, ButtonTextColor, ButtonTextFontSize);
                canvas.DrawString(ButtonText, 0, 0, (float)this.Width, (float)this.Height, ButtonTextHorizontalAlignment, ButtonTextVerticalAlignment);
            }
        }

        public void LoadButtonBackgroundImages(Assembly assembly, string? manifestResourcePathButtonBackgroundImageNotPressed, 
            string? manifestResourcePathButtonBackgroundPressedImage)
        {
            if (!string.IsNullOrEmpty(manifestResourcePathButtonBackgroundImageNotPressed))
            {
                ButtonBackgroundNotPressedImage = Helper.LoadImage(assembly, manifestResourcePathButtonBackgroundImageNotPressed);
            }
            if (!string.IsNullOrEmpty(manifestResourcePathButtonBackgroundPressedImage))
            {
                ButtonBackgroundPressedImage = Helper.LoadImage(assembly, manifestResourcePathButtonBackgroundPressedImage);
            }
        }
    }
}
