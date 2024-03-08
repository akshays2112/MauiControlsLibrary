using System.Reflection;

namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a combobox with images in the Maui Controls Library.
    /// </summary>
    public class MCLImageCombobox : MCLComboboxBase
    {
        /// <summary>
        /// Gets or sets the image of the combobox button when it is not pressed.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? ComboboxButtonNotPressedImage { get; set; }

        /// <summary>
        /// Gets or sets the image of the combobox button when it is pressed.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? ComboboxButtonPressedImage { get; set; }

        /// <summary>
        /// Draws the button of the combobox on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the button.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="height">The height of the button.</param>
        public override void DrawButton(ICanvas canvas, float x, float y, float width, float height)
        {
            Microsoft.Maui.Graphics.IImage? image = ButtonTapped ? ComboboxButtonPressedImage : ComboboxButtonNotPressedImage;
            if (image != null)
                canvas.DrawImage(image, x, y, width, height);
        }

        /// <summary>
        /// Loads the images of the combobox button from the specified assembly and manifest resource paths.
        /// </summary>
        /// <param name="assembly">The assembly that contains the images.</param>
        /// <param name="manifestResourcePathComboboxButtonImageNotPressed">The manifest resource path of the not pressed image.</param>
        /// <param name="manifestResourcePathComboboxButtonPressedImage">The manifest resource path of the pressed image.</param>
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
