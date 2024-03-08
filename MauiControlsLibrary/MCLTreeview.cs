namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a treeview control in the Maui Controls Library.
    /// </summary>
    public class MCLTreeview : MCLTreeviewBase
    {
        /// <summary>
        /// Gets or sets the label for the expand button.
        /// </summary>
        public string ExpandButtonLabel { get; set; } = "+";

        /// <summary>
        /// Gets or sets the label for the collapse button.
        /// </summary>
        public string CollapseButtonLabel { get; set; } = "-";

        /// <summary>
        /// Draws the expand/collapse button for a treeview node.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="levelIndent">The indentation level of the node.</param>
        /// <param name="heightOffset">The height offset of the node.</param>
        /// <param name="node">The treeview node for which to draw the button.</param>
        public override void DrawExpandCollapseButton(ICanvas canvas, int levelIndent, int heightOffset, TreeviewNode node)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRectangle(levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, ExpandCollapseButtonWidthHeight);
            canvas.DrawString(node.ExpandCollapseButtonState == ExpandCollapseButtonState.Expanded ? CollapseButtonLabel : ExpandButtonLabel,
                levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, ExpandCollapseButtonWidthHeight, HorizontalAlignment.Center, VerticalAlignment.Center);
        }
    }
}
