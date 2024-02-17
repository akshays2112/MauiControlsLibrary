using System.Reflection;

namespace MauiControlsLibrary
{
    public abstract class MCLTreeviewBase : GraphicsView, IDrawable
    {
        public TreeviewNode[]? TreeviewNodes { get; set; }
        public Color? TreeviewBackgroundColor { get; set; }
        public Helper.StandardFontPropterties TreeviewNodeLabelFont { get; set; } = new();
        public int PerLevelNodeLeftIndent { get; set; } = 15;
        public int SpacingBetweenButtonAndLabel { get; set; } = 20;
        public int TreeviewNodeHeight { get; set; } = 35;
        public int ExpandCollapseButtonWidthHeight { get; set; } = 20;
        public event EventHandler<TreeviewNodeLabelTappedEventArgs>? OnMCLTreeviewNodeLabelTapped;

        protected List<TreeviewNodeHit> treeviewNodeHits { get; set; } = [];
        protected int currentPanY = 0;
        protected int currentPanX = 0;
        protected int maxTreeviewNodeWidth = 0;
        protected int maxTreeviewNodesHeight = 0;

        public class TreeviewNodeLabelTappedEventArgs(EventArgs? eventArgs, TreeviewNode? treeviewNode) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public TreeviewNode? TreeviewNode { get; set; } = treeviewNode;
        }

        public class TreeviewNode(string label, ExpandCollapseButtonState expandCollapseButtonState = ExpandCollapseButtonState.Collapsed,
            TreeviewNode[]? childNodes = null)
        {
            public string? Label { get; set; } = label;
            public ExpandCollapseButtonState ExpandCollapseButtonState { get; set; } = expandCollapseButtonState;
            public TreeviewNode[]? ChildNodes { get; set; } = childNodes ?? Array.Empty<TreeviewNode>();
        }

        public enum ExpandCollapseButtonState
        {
            Expanded,
            Collapsed
        }

        protected class TreeviewNodeHit(RectF hitArea, HitAreaType hitAreaType, TreeviewNode treeviewNode)
        {
            public RectF HitArea { get; set; } = hitArea;
            public HitAreaType? HitAreaType { get; set; } = hitAreaType;
            public TreeviewNode? TreeviewNode { get; set; } = treeviewNode;
        }

        protected enum HitAreaType
        {
            ExpandCollapseButton,
            Label
        }

        public MCLTreeviewBase()
        {
            Drawable = this;
            PanGestureRecognizer panGesture = new();
            panGesture.PanUpdated += PanGesture_PanUpdated;
            GestureRecognizers.Add(panGesture);
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(this);
            if (Helper.PointFValueIsInRange(point, 0, Width, 0, Height))
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
                                treeviewNodeHits[i].TreeviewNode.ExpandCollapseButtonState = treeviewNodeHits[i].TreeviewNode.ExpandCollapseButtonState == ExpandCollapseButtonState.Collapsed
                                    ? ExpandCollapseButtonState.Expanded
                                    : ExpandCollapseButtonState.Collapsed;
                                Invalidate();
                                break;
                            }
                            else
                            {
                                OnMCLTreeviewNodeLabelTapped?.Invoke(this, new TreeviewNodeLabelTappedEventArgs(e, treeviewNodeHits[i].TreeviewNode));
                            }
                        }
                    }
                }
            }
        }

        public virtual void PanGesture_PanUpdated(object? sender, PanUpdatedEventArgs e)
        {
            if (treeviewNodeHits != null && e.StatusType == GestureStatus.Running)
            {
                currentPanY += (int)e.TotalY;
                currentPanY = Helper.ValueResetOnBoundsCheck(currentPanY, 0, maxTreeviewNodesHeight - TreeviewNodeHeight);
                currentPanX += (int)e.TotalX;
                currentPanX = Helper.ValueResetOnBoundsCheck(currentPanX, 0, maxTreeviewNodeWidth - (int)Width + TreeviewNodeLabelFont.FontSize);
                Invalidate();
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
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

        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height) {
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

        public virtual void DrawExpandCollapseButton(ICanvas canvas, int levelIndent, int heightOffset, TreeviewNode node) { }

        public virtual void DrawTreeviewNodeLabel(ICanvas canvas, TreeviewNode node, float x, float y, float width, float height)
        {
            canvas.DrawString(node.Label, x, y, width, height, HorizontalAlignment.Left, VerticalAlignment.Center);
        }
    }
}
