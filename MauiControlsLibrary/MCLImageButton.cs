using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiControlsLibrary
{
    public class MCLImageButton : MCLButton
    {
        public new string? ButtonText { get; }
        public new Color? ButtonColor { get; }
        public new Color? ButtonTappedColor { get; }
        public new double? ButtonCornerRadius { get; }
        public new Helper.StandardFontPropterties? ButtonLabelFont { get; }
        public Microsoft.Maui.Graphics.IImage? ButtonBackgroundNotPressedImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? ButtonBackgroundPressedImage { get; set; }
        public bool ClipButtonBackgroundImage { get; set; } = true;

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (Tapped)
            {
                Tapped = false;
                if (ButtonBackgroundPressedImage != null)
                {
                    canvas.SaveState();
                    canvas.ClipRectangle(0, 0, (float)Width, (float)Height);
                    if (ClipButtonBackgroundImage)
                    {
                        canvas.DrawImage(ButtonBackgroundPressedImage, 0, 0, ButtonBackgroundPressedImage.Width < Width ? (float)Width :
                            ButtonBackgroundPressedImage.Width, ButtonBackgroundPressedImage.Height < Height ? (float)Height :
                            ButtonBackgroundPressedImage.Height);
                    }
                    else
                        canvas.DrawImage(ButtonBackgroundPressedImage, 0, 0, (float)Width, (float)Height);
                    canvas.ResetState();
                    Invalidate();
                }
            }
            else if (ButtonBackgroundNotPressedImage != null)
            {
                canvas.SaveState();
                canvas.ClipRectangle(0, 0, (float)Width, (float)Height);
                if (ClipButtonBackgroundImage)
                {
                    canvas.DrawImage(ButtonBackgroundNotPressedImage, 0, 0, ButtonBackgroundNotPressedImage.Width < Width ? (float)Width :
                        ButtonBackgroundNotPressedImage.Width, ButtonBackgroundNotPressedImage.Height < Height ? (float)Height :
                        ButtonBackgroundNotPressedImage.Height);
                }
                else
                    canvas.DrawImage(ButtonBackgroundNotPressedImage, 0, 0, (float)Width, (float)Height);
                canvas.ResetState();
            }
        }

        public void LoadButtonBackgroundImages(Assembly assembly, string? manifestResourcePathButtonBackgroundImageNotPressed,
            string? manifestResourcePathButtonBackgroundPressedImage)
        {
            ButtonBackgroundNotPressedImage = Helper.LoadImage(assembly, manifestResourcePathButtonBackgroundImageNotPressed);
            ButtonBackgroundPressedImage = Helper.LoadImage(assembly, manifestResourcePathButtonBackgroundPressedImage);
        }
    }
}
