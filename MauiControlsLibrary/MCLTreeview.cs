namespace MauiControlsLibrary
{
    public class MCLTreeview : MCLTreeviewBase
    {
        public string ExpandButtonLabel { get; set; } = "+";
        public string CollapseButtonLabel { get; set; } = "-";

        public override void DrawExpandCollapseButton(ICanvas canvas, int levelIndent, int heightOffset, TreeviewNode node)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, ExpandCollapseButtonWidthHeight);
            canvas.DrawString(node.ExpandCollapseButtonState == ExpandCollapseButtonState.Expanded ? CollapseButtonLabel : ExpandButtonLabel,
                levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, ExpandCollapseButtonWidthHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
        }
    }
}
