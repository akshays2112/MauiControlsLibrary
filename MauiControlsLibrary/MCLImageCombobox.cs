using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImageCombobox : MCLComboboxBase
    {
        public Microsoft.Maui.Graphics.IImage? ComboboxButtonNotPressedImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? ComboboxButtonPressedImage { get; set; }

        public override void DrawButton(ICanvas canvas, float x, float y, float width, float height)
        {
            Microsoft.Maui.Graphics.IImage? image = ButtonTapped ? ComboboxButtonPressedImage : ComboboxButtonNotPressedImage;
            if (image != null)
                canvas.DrawImage(image, x, y, width, height);
        }

        public void LoadComboboxButtonImages(Assembly assembly, string? manifestResourcePathComboboxButtonImageNotPressed,
            string? manifestResourcePathComboboxButtonPressedImage)
        {
            ComboboxButtonNotPressedImage = Helper.LoadImage(assembly, manifestResourcePathComboboxButtonImageNotPressed);
            if (ComboboxButtonNotPressedImage != null)
            {
                ButtonWidth = (int)ComboboxButtonNotPressedImage.Width;
                ButtonHeight = (int)ComboboxButtonNotPressedImage.Height;
            }
            ComboboxButtonPressedImage = Helper.LoadImage(assembly, manifestResourcePathComboboxButtonPressedImage);
        }
    }
}
