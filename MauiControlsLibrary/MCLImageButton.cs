using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImageButton : MCLButtonBase
    {
        public Microsoft.Maui.Graphics.IImage? ButtonBackgroundNotPressedImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? ButtonBackgroundPressedImage { get; set; }
        public bool ClipButtonBackgroundImage { get; set; } = true;

        public override void DrawButton(ICanvas canvas, float x, float y, float width, float height)
        {
            if (ButtonBackgroundPressedImage != null && ButtonBackgroundNotPressedImage != null)
            {
                Microsoft.Maui.Graphics.IImage image = Tapped ? ButtonBackgroundPressedImage : ButtonBackgroundNotPressedImage;
                canvas.SaveState();
                canvas.ClipRectangle(x, y , width, height);
                if (ClipButtonBackgroundImage)
                {
                    canvas.DrawImage(image, x, y, image.Width < width ? width : image.Width, image.Height < height ? height : image.Height);
                }
                else
                {
                    canvas.DrawImage(image, x, y, width, height);
                }
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
