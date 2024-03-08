using System.Reflection;

namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents an image button in the Maui Controls Library.
    /// </summary>
    public class MCLImageButton : MCLButtonBase
    {
        /// <summary>
        /// Gets or sets the image to display when the button is not pressed.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? ButtonBackgroundNotPressedImage { get; set; }

        /// <summary>
        /// Gets or sets the image to display when the button is pressed.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? ButtonBackgroundPressedImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the button background image should be clipped.
        /// </summary>
        public bool ClipButtonBackgroundImage { get; set; } = true;

        /// <summary>
        /// Draws the button on the specified canvas at the specified location and with the specified size.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the button.</param>
        /// <param name="y">The y-coordinate of the button.</param>
        /// <param name="width">The width of the button.</param>
        /// <param name="height">The height of the button.</param>
        public override void DrawButton(ICanvas canvas, float x, float y, float width, float height)
        {
            if (ButtonBackgroundPressedImage != null && ButtonBackgroundNotPressedImage != null)
            {
                Microsoft.Maui.Graphics.IImage image = Tapped ? ButtonBackgroundPressedImage : ButtonBackgroundNotPressedImage;
                canvas.SaveState();
                canvas.ClipRectangle(x, y, width, height);
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

        /// <summary>
        /// Loads the images for the button background from the specified assembly and paths.
        /// </summary>
        /// <param name="assembly">The assembly to load the images from.</param>
        /// <param name="manifestResourcePathButtonBackgroundImageNotPressed">The path to the image for the button background when not pressed.</param>
        /// <param name="manifestResourcePathButtonBackgroundPressedImage">The path to the image for the button background when pressed.</param>
        public void LoadButtonBackgroundImages(Assembly assembly, string? manifestResourcePathButtonBackgroundImageNotPressed,
            string? manifestResourcePathButtonBackgroundPressedImage)
        {
            ButtonBackgroundNotPressedImage = Helper.LoadImage(assembly, manifestResourcePathButtonBackgroundImageNotPressed);
            ButtonBackgroundPressedImage = Helper.LoadImage(assembly, manifestResourcePathButtonBackgroundPressedImage);
        }
    }
}
