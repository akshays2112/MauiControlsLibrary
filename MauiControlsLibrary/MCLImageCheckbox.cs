using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImageCheckbox : MCLCheckboxBase
    {
        public Microsoft.Maui.Graphics.IImage? CheckboxCheckedImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? CheckboxUncheckedImage { get; set; }
        public bool StrecthCheckboxImage { get; set; } = false;

        public override void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            if (CheckboxCheckedImage != null && CheckboxUncheckedImage != null)
            {
                Microsoft.Maui.Graphics.IImage image = IsChecked ? CheckboxCheckedImage : CheckboxUncheckedImage;
                canvas.DrawImage(image, x, y, StrecthCheckboxImage ? width : image.Width, StrecthCheckboxImage ? height : image.Height);
            }
        }

        public void LoadCheckboxImages(Assembly assembly, string? manifestResourcePathCheckboxUnchecked,
            string? manifestResourcePathCheckboxCheckedImage)
        {
            CheckboxUncheckedImage = Helper.LoadImage(assembly, manifestResourcePathCheckboxUnchecked);
            CheckboxCheckedImage = Helper.LoadImage(assembly, manifestResourcePathCheckboxCheckedImage);
        }
    }
}
