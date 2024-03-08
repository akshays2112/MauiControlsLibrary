namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents the base class for a treeview control in the Maui Controls Library.
    /// </summary>
    public abstract class MCLTreeviewBase : GraphicsView, IDrawable
    {
        /// <summary>
        /// Gets or sets the nodes of the treeview.
        /// </summary>
        public TreeviewNode[]? TreeviewNodes { get; set; }

        /// <summary>
        /// Gets or sets the background color of the treeview.
        /// </summary>
        public Color? TreeviewBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the font properties of the node labels in the treeview.
        /// </summary>
        public Helper.StandardFontPropterties TreeviewNodeLabelFont { get; set; } = new();

        /// <summary>
        /// Gets or sets the left indentation per level of the nodes in the treeview.
        /// </summary>
        public int PerLevelNodeLeftIndent { get; set; } = 15;

        /// <summary>
        /// Gets or sets the spacing between the expand/collapse button and the label of a node in the treeview.
        /// </summary>
        public int SpacingBetweenButtonAndLabel { get; set; } = 20;

        /// <summary>
        /// Gets or sets the height of the nodes in the treeview.
        /// </summary>
        public int TreeviewNodeHeight { get; set; } = 35;

        /// <summary>
        /// Gets or sets the width and height of the expand/collapse button in the treeview.
        /// </summary>
        public int ExpandCollapseButtonWidthHeight { get; set; } = 20;

        /// <summary>
        /// Occurs when a node label in the treeview is tapped.
        /// </summary>
        public event EventHandler<TreeviewNodeLabelTappedEventArgs>? OnMCLTreeviewNodeLabelTapped;

        protected List<TreeviewNodeHit> treeviewNodeHits { get; set; } = [];
        protected int currentPanY = 0;
        protected int currentPanX = 0;
        protected int maxTreeviewNodeWidth = 0;
        protected int maxTreeviewNodesHeight = 0;

        /// <summary>
        /// Represents the arguments for the OnMCLTreeviewNodeLabelTapped event.
        /// </summary>
        public class TreeviewNodeLabelTappedEventArgs(EventArgs? eventArgs, TreeviewNode? treeviewNode) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public TreeviewNode? TreeviewNode { get; set; } = treeviewNode;
        }

        /// <summary>
        /// Represents a node in the treeview.
        /// </summary>
        public class TreeviewNode(string label, ExpandCollapseButtonState expandCollapseButtonState = ExpandCollapseButtonState.Collapsed,
            TreeviewNode[]? childNodes = null)
        {
            public string? Label { get; set; } = label;
            public ExpandCollapseButtonState ExpandCollapseButtonState { get; set; } = expandCollapseButtonState;
            public TreeviewNode[]? ChildNodes { get; set; } = childNodes ?? Array.Empty<TreeviewNode>();
        }

        /// <summary>
        /// Specifies the states of the expand/collapse button in the treeview.
        /// </summary>
        public enum ExpandCollapseButtonState
        {
            Expanded,
            Collapsed
        }

        /// <summary>
        /// Represents a hit area in the treeview.
        /// </summary>
        public class TreeviewNodeHit(RectF hitArea, HitAreaType hitAreaType, TreeviewNode treeviewNode)
        {
            public RectF HitArea { get; set; } = hitArea;
            public HitAreaType? HitAreaType { get; set; } = hitAreaType;
            public TreeviewNode? TreeviewNode { get; set; } = treeviewNode;
        }

        /// <summary>
        /// Specifies the types of hit areas in the treeview.
        /// </summary>
        public enum HitAreaType
        {
            ExpandCollapseButton,
            Label
        }

        /// <summary>
        /// Initializes a new instance of the MCLTreeviewBase class.
        /// </summary>
        public MCLTreeviewBase()
        {
            Drawable = this;
            Helper.CreatePanGestureRecognizer(PanGesture_PanUpdated, GestureRecognizers);
            Helper.CreateTapGestureRecognizer(TapGestureRecognizer_Tapped, GestureRecognizers);
        }

        /// <summary>
        /// Handles the Tapped event of the TapGestureRecognizer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            TreeviewTapped(0, (float)Width, 0, (float)Height, treeviewNodeHits, this, e, Invalidate, OnMCLTreeviewNodeLabelTapped);
        }

        /// <summary>
        /// Handles the tap event on the treeview.
        /// </summary>
        /// <param name="x">The x-coordinate of the tap location.</param>
        /// <param name="width">The width of the treeview.</param>
        /// <param name="y">The y-coordinate of the tap location.</param>
        /// <param name="height">The height of the treeview.</param>
        /// <param name="treeviewNodeHits">The list of treeview node hits.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        /// <param name="invalidate">The action to invalidate the treeview.</param>
        /// <param name="onMCLTreeviewNodeLabelTapped">The event handler for the MCLTreeviewNodeLabelTapped event.</param>
        public static void TreeviewTapped(float x, float width, float y, float height, List<TreeviewNodeHit> treeviewNodeHits,
            GraphicsView sender, TappedEventArgs e, Action invalidate, EventHandler<TreeviewNodeLabelTappedEventArgs>? onMCLTreeviewNodeLabelTapped)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.PointFValueIsInRange(point, x, width, y, height))
            {
                if (treeviewNodeHits != null)
                {
                    for (int i = 0; i < treeviewNodeHits.Count; i++)
                    {
                        if (treeviewNodeHits[i] != null && treeviewNodeHits[i].TreeviewNode != null &&
                            Helper.PointFValueIsInRange(point, treeviewNodeHits[i].HitArea.X, treeviewNodeHits[i].HitArea.X +
                            treeviewNodeHits[i].HitArea.Width, treeviewNodeHits[i].HitArea.Y, treeviewNodeHits[i].HitArea.Y +
                            treeviewNodeHits[i].HitArea.Height))
                        {
                            if (treeviewNodeHits[i].HitAreaType == HitAreaType.ExpandCollapseButton)
                            {
                                treeviewNodeHits[i].TreeviewNode.ExpandCollapseButtonState = treeviewNodeHits[i].TreeviewNode.ExpandCollapseButtonState ==
                                    ExpandCollapseButtonState.Collapsed ? ExpandCollapseButtonState.Expanded : ExpandCollapseButtonState.Collapsed;
                                invalidate();
                                break;
                            }
                            else
                            {
                                onMCLTreeviewNodeLabelTapped?.Invoke(sender, new TreeviewNodeLabelTappedEventArgs(e, treeviewNodeHits[i].TreeviewNode));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the PanUpdated event of the PanGestureRecognizer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PanUpdatedEventArgs"/> instance containing the event data.</param>
        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            TreeviewPan(treeviewNodeHits, ref currentPanY, ref currentPanX, maxTreeviewNodeWidth, maxTreeviewNodesHeight, TreeviewNodeHeight, Width,
                TreeviewNodeLabelFont, e, Invalidate);
        }

        /// <summary>
        /// Handles the pan gesture on the treeview.
        /// </summary>
        /// <param name="treeviewNodeHits">The list of treeview node hits.</param>
        /// <param name="currentPanY">The current pan y-coordinate.</param>
        /// <param name="currentPanX">The current pan x-coordinate.</param>
        /// <param name="maxTreeviewNodeWidth">The maximum width of the treeview nodes.</param>
        /// <param name="maxTreeviewNodesHeight">The maximum height of the treeview nodes.</param>
        /// <param name="treeviewNodeHeight">The height of the treeview nodes.</param>
        /// <param name="width">The width of the treeview.</param>
        /// <param name="treeviewNodeLabelFont">The font properties of the treeview node labels.</param>
        /// <param name="e">The <see cref="PanUpdatedEventArgs"/> instance containing the event data.</param>
        /// <param name="invalidate">The action to invalidate the treeview.</param>
        public static void TreeviewPan(List<TreeviewNodeHit> treeviewNodeHits, ref int currentPanY, ref int currentPanX,
            int maxTreeviewNodeWidth, int maxTreeviewNodesHeight, int treeviewNodeHeight, double width, Helper.StandardFontPropterties treeviewNodeLabelFont,
            PanUpdatedEventArgs e, Action invalidate)
        {
            if (treeviewNodeHits != null && e.StatusType == GestureStatus.Running)
            {
                currentPanY += (int)e.TotalY;
                currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, maxTreeviewNodesHeight - treeviewNodeHeight);
                currentPanX += (int)e.TotalX;
                currentPanX = Helper.ValueResetOnBoundsCheck(currentPanX, 0, maxTreeviewNodeWidth - (int)width + treeviewNodeLabelFont.FontSize);
                invalidate();
            }
        }

        /// <summary>
        /// Draws the treeview on the provided canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to draw the treeview.</param>
        /// <param name="dirtyRect">The area of the canvas that needs to be redrawn.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            maxTreeviewNodesHeight = 0;
            treeviewNodeHits.Clear();
            if (Helper.ArrayNotNullOrEmpty(TreeviewNodes))
            {
                int level = 0;
                int yOffset = 0;
                canvas.SaveState();
                canvas.ClipRectangle(0, 0, (float)Width, (float)Height);
                for (int i = 0; i < TreeviewNodes?.Length; i++)
                {
                    yOffset = DrawTreeviewNode(canvas, TreeviewNodes[i], level, yOffset);
                }
                canvas.ResetState();
            }
        }

        /// <summary>
        /// Draws a node of the treeview on the provided canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to draw the node.</param>
        /// <param name="node">The node to draw.</param>
        /// <param name="level">The level of the node in the treeview.</param>
        /// <param name="yOffset">The y-coordinate offset for drawing the node.</param>
        /// <returns>The updated y-coordinate offset after drawing the node.</returns>
        private int DrawTreeviewNode(ICanvas canvas, TreeviewNode node, int level, int yOffset)
        {
            int levelIndent = (level * PerLevelNodeLeftIndent) - currentPanX;
            SizeF nodeLabelSize = canvas.GetStringSize(node.Label, TreeviewNodeLabelFont.Font, TreeviewNodeLabelFont.FontSize);
            Helper.SetFontAttributes(canvas, TreeviewNodeLabelFont);
            if (Helper.ArrayNotNullOrEmpty(node.ChildNodes))
            {
                int heightOffset = yOffset + ((TreeviewNodeHeight - ExpandCollapseButtonWidthHeight) / 2) - currentPanY;
                DrawExpandCollapseButton(canvas, levelIndent, heightOffset, node);
                treeviewNodeHits.Add(new TreeviewNodeHit(new RectF(levelIndent, heightOffset, ExpandCollapseButtonWidthHeight,
                    ExpandCollapseButtonWidthHeight), HitAreaType.ExpandCollapseButton, node));
            }
            if (node.Label != null)
            {
                int tmpWidth = levelIndent + ExpandCollapseButtonWidthHeight + SpacingBetweenButtonAndLabel;
                DrawTreeviewNodeLabel(canvas, node, tmpWidth, yOffset - currentPanY, tmpWidth + (int)nodeLabelSize.Width, TreeviewNodeHeight);
            }
            treeviewNodeHits.Add(new TreeviewNodeHit(new RectF(levelIndent + ExpandCollapseButtonWidthHeight + SpacingBetweenButtonAndLabel, yOffset - currentPanY,
                (float)Width - levelIndent - ExpandCollapseButtonWidthHeight - SpacingBetweenButtonAndLabel, TreeviewNodeHeight), HitAreaType.Label, node));
            if (maxTreeviewNodeWidth < levelIndent + ExpandCollapseButtonWidthHeight + SpacingBetweenButtonAndLabel + nodeLabelSize.Width)
            {
                maxTreeviewNodeWidth = levelIndent + ExpandCollapseButtonWidthHeight + SpacingBetweenButtonAndLabel + (int)Math.Ceiling((double)nodeLabelSize.Width);
            }
            yOffset += TreeviewNodeHeight;
            if (node.ChildNodes != null && node.ExpandCollapseButtonState == ExpandCollapseButtonState.Expanded)
            {
                for (int i = 0; i < node.ChildNodes.Length; i++)
                {
                    yOffset = DrawTreeviewNode(canvas, node.ChildNodes[i], level + 1, yOffset);
                }
            }
            if (yOffset > maxTreeviewNodesHeight)
            {
                maxTreeviewNodesHeight = yOffset;
            }
            return yOffset;
        }

        /// <summary>
        /// Draws the frame of the treeview on the provided canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to draw the frame.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the frame.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the frame.</param>
        /// <param name="width">The width of the frame.</param>
        /// <param name="height">The height of the frame.</param>
        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            if (TreeviewBackgroundColor != null)
            {
                canvas.FillColor = TreeviewBackgroundColor;
                canvas.FillRectangle(new Rect(x, y, width, height));
            }
            else
            {
                canvas.StrokeColor = Colors.Grey;
                canvas.DrawRectangle(x, y, width, height);
            }
        }

        /// <summary>
        /// Draws the expand/collapse button of a node on the provided canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to draw the button.</param>
        /// <param name="levelIndent">The indentation level of the node.</param>
        /// <param name="heightOffset">The y-coordinate offset for drawing the button.</param>
        /// <param name="node">The node whose button to draw.</param>
        public virtual void DrawExpandCollapseButton(ICanvas canvas, int levelIndent, int heightOffset, TreeviewNode node) { }

        /// <summary>
        /// Draws the label of a node on the provided canvas.
        /// </summary>
        /// <param name="canvas">The canvas on which to draw the label.</param>
        /// <param name="node">The node whose label to draw.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the label.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the label.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="height">The height of the label.</param>
        public virtual void DrawTreeviewNodeLabel(ICanvas canvas, TreeviewNode node, float x, float y, float width, float height)
        {
            canvas.DrawString(node.Label, x, y, width, height, HorizontalAlignment.Left, VerticalAlignment.Center);
        }
    }
}
