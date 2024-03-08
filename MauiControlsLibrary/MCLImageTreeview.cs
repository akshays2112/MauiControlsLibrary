using System.Reflection;

namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a treeview with images in the Maui Controls Library.
    /// </summary>
    public class MCLImageTreeview : MCLTreeviewBase
    {
        /// <summary>
        /// Gets or sets the image of the expand button.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? ExpandButtonImage { get; set; }

        /// <summary>
        /// Gets or sets the image of the collapse button.
        /// </summary>
        public Microsoft.Maui.Graphics.IImage? CollapseButtonImage { get; set; }

        /// <summary>
        /// Draws the expand/collapse button of the treeview node on the canvas.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="levelIndent">The indentation level of the node.</param>
        /// <param name="heightOffset">The height offset of the node.</param>
        /// <param name="node">The treeview node.</param>
        public override void DrawExpandCollapseButton(ICanvas canvas, int levelIndent, int heightOffset, TreeviewNode node)
        {
            if (ExpandButtonImage != null && CollapseButtonImage != null)
            {
                canvas.DrawImage(node.ExpandCollapseButtonState == ExpandCollapseButtonState.Expanded ? CollapseButtonImage : ExpandButtonImage,
                    levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, ExpandCollapseButtonWidthHeight);
            }
        }

        /// <summary>
        /// Loads the images of the expand/collapse buttons from the specified assembly and manifest resource paths.
        /// </summary>
        /// <param name="assembly">The assembly that contains the images.</param>
        /// <param name="manifestResourcePathExpandImage">The manifest resource path of the expand button image.</param>
        /// <param name="manifestResourcePathCollapseImage">The manifest resource path of the collapse button image.</param>
        public void LoadExpandCollapseImages(Assembly assembly, string manifestResourcePathExpandImage, string manifestResourcePathCollapseImage)
        {
            ExpandButtonImage = Helper.LoadImage(assembly, manifestResourcePathExpandImage);
            if (ExpandButtonImage != null)
            {
                ExpandCollapseButtonWidthHeight = (int)ExpandButtonImage.Width;
            }

            CollapseButtonImage = Helper.LoadImage(assembly, manifestResourcePathCollapseImage);
        }
    }
}
