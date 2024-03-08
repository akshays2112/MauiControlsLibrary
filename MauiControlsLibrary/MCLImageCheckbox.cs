using System.Reflection;

namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a checkbox with images in the Maui Controls Library.
    /// </summary>
    public class MCLImageCheckbox : MCLCheckboxBase
    {
        /// <summary>
        /// Gets or sets the image of the checkbox when it is checked.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? CheckboxCheckedImage { get; set; }

        /// <summary>
        /// Gets or sets the image of the checkbox when it is unchecked.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? CheckboxUncheckedImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the checkbox image is stretched.
        /// </summary>
        public bool StrecthCheckboxImage { get; set; } = false;

        /// <summary>
        /// Draws the frame of the checkbox on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the checkbox.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the checkbox.</param>
        /// <param name="width">The width of the checkbox.</param>
        /// <param name="height">The height of the checkbox.</param>
        public override void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            if (CheckboxCheckedImage != null && CheckboxUncheckedImage != null)
            {
                Microsoft.Maui.Graphics.IImage image = IsChecked ? CheckboxCheckedImage : CheckboxUncheckedImage;
                canvas.DrawImage(image, x, y, StrecthCheckboxImage ? width : image.Width, StrecthCheckboxImage ? height : image.Height);
            }
        }

        /// <summary>
        /// Loads the images of the checkbox from the specified assembly and manifest resource paths.
        /// </summary>
        /// <param name="assembly">The assembly that contains the images.</param>
        /// <param name="manifestResourcePathCheckboxUnchecked">The manifest resource path of the unchecked image.</param>
        /// <param name="manifestResourcePathCheckboxCheckedImage">The manifest resource path of the checked image.</param>
        public void LoadCheckboxImages(Assembly assembly, string? manifestResourcePathCheckboxUnchecked,
            string? manifestResourcePathCheckboxCheckedImage)
        {
            CheckboxUncheckedImage = Helper.LoadImage(assembly, manifestResourcePathCheckboxUnchecked);
            CheckboxCheckedImage = Helper.LoadImage(assembly, manifestResourcePathCheckboxCheckedImage);
        }
    }
}
