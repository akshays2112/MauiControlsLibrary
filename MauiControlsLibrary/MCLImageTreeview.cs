using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImageTreeview : MCLTreeviewBase
    {
        public Microsoft.Maui.Graphics.IImage? ExpandButtonImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? CollapseButtonImage { get; set; }

        public override void DrawExpandCollapseButton(ICanvas canvas, int levelIndent, int heightOffset, TreeviewNode node)
        {
            if (ExpandButtonImage != null && CollapseButtonImage != null)
            {
                canvas.DrawImage(node.ExpandCollapseButtonState == ExpandCollapseButtonState.Expanded ? CollapseButtonImage : ExpandButtonImage, 
                    levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, ExpandCollapseButtonWidthHeight);
            }
        }

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
