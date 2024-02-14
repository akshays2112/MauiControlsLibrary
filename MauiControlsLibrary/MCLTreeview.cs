using Microsoft.Maui.Graphics.Platform;
using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLTreeview : GraphicsView, IDrawable
    {
        public TreeviewNode[]? TreeviewNodes { get; set; }
        public string ExpandButtonLabel { get; set; } = "+";
        public string CollapseButtonLabel { get; set; } = "-";
        public Microsoft.Maui.Graphics.IImage? ExpandButtonImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? CollapseButtonImage { get; set; }
        public Color? TreeviewBackgroundColor { get; set; }
        public Microsoft.Maui.Graphics.Font TreeviewNodeLabelFont { get; set; } = new Microsoft.Maui.Graphics.Font("Arial");
        public Color TreeviewNodeLabelColor { get; set; } = Colors.Black;
        public int TreeviewNodeLabelFontSize { get; set; } = 18;
        public HorizontalAlignment TreeviewNodeLabelHorizontalAlignment { get; set; } = HorizontalAlignment.Center;
        public VerticalAlignment TreeviewNodeLabelVerticalAlignment { get; set; } = VerticalAlignment.Center;
        public int PerLevelNodeLeftIndent { get; set; } = 15;
        public int SpacingBetweenButtonAndLabel { get; set; } = 20;
        public int TreeviewNodeHeight { get; set; } = 35;
        public int ExpandCollapseButtonWidthHeight { get; set; } = 20;
        public event EventHandler<TreeviewNodeLabelTappedEventArgs>? OnMCLTreeviewNodeLabelTapped;

        private List<TreeviewNodeHit> treeviewNodeHits { get; set; } = new();
        private int currentPanY = 0;
        private int currentPanX = 0;
        private int maxTreeviewNodeWidth = 0;
        private int maxTreeviewNodesHeight = 0;

        public class TreeviewNodeLabelTappedEventArgs : EventArgs
        {
            public EventArgs? EventArgs { get; set; }
            public TreeviewNode? TreeviewNode { get; set; }

            public TreeviewNodeLabelTappedEventArgs(EventArgs? eventArgs, TreeviewNode? treeviewNode)
            {
                EventArgs = eventArgs;
                TreeviewNode = treeviewNode;
            }
        }

        public class TreeviewNode
        {
            public string? Label { get; set; }
            public ExpandCollapseButtonState ExpandCollapseButtonState { get; set; } = ExpandCollapseButtonState.Collapsed;
            public TreeviewNode[]? ChildNodes { get; set; }

            public TreeviewNode(string label, ExpandCollapseButtonState expandCollapseButtonState, TreeviewNode[]? childNodes)
            {
                Label = label;
                ExpandCollapseButtonState = expandCollapseButtonState;
                ChildNodes = childNodes;
            }
        }

        public enum ExpandCollapseButtonState
        {
            Expanded,
            Collapsed
        }

        private class TreeviewNodeHit
        {
            public RectF HitArea { get; set; }
            public HitAreaType? HitAreaType { get; set; }
            public TreeviewNode? TreeviewNode { get; set; }

            public TreeviewNodeHit(RectF hitArea, HitAreaType hitAreaType, TreeviewNode treeviewNode)
            {
                HitArea = hitArea;
                HitAreaType = hitAreaType;
                TreeviewNode = treeviewNode;
            }
        }

        private enum HitAreaType
        {
            ExpandCollapseButton,
            Label
        }

        public MCLTreeview()
        {
            this.Drawable = this;
            PanGestureRecognizer panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += (s, e) => {
                if (treeviewNodeHits != null && e.StatusType == GestureStatus.Running)
                {
                    currentPanY += (int)e.TotalY;
                    if (currentPanY < 0)
                        currentPanY = 0;
                    if (currentPanY > maxTreeviewNodesHeight - TreeviewNodeHeight)
                        currentPanY = maxTreeviewNodesHeight - TreeviewNodeHeight;
                    currentPanX += (int)e.TotalX;
                    if (currentPanX < 0)
                        currentPanX = 0;
                    if (currentPanX > maxTreeviewNodeWidth - (int)this.Width + TreeviewNodeLabelFontSize)
                        currentPanX = maxTreeviewNodeWidth - (int)this.Width + TreeviewNodeLabelFontSize;
                    this.Invalidate();
                }
            };
            this.GestureRecognizers.Add(panGesture);
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (point.HasValue && point.Value.X >= 0 && point.Value.X <= this.Width
                    && point.Value.Y >= 0 && point.Value.Y < this.Height)
                {
                    if (treeviewNodeHits != null) {
                        for (int i = 0; i < treeviewNodeHits.Count; i++)
                        {
                            if (treeviewNodeHits[i] != null && treeviewNodeHits[i].TreeviewNode != null && point.Value.X >= treeviewNodeHits[i].HitArea.X &&
                                point.Value.X <= treeviewNodeHits[i].HitArea.X + treeviewNodeHits[i].HitArea.Width &&
                                point.Value.Y >= treeviewNodeHits[i].HitArea.Y && point.Value.Y <= treeviewNodeHits[i].HitArea.Y + 
                                treeviewNodeHits[i].HitArea.Height)
                            {
                                if (treeviewNodeHits[i].HitAreaType == HitAreaType.ExpandCollapseButton)
                                {
                                    if (treeviewNodeHits[i].TreeviewNode.ExpandCollapseButtonState == ExpandCollapseButtonState.Collapsed)
                                    {
                                        treeviewNodeHits[i].TreeviewNode.ExpandCollapseButtonState = ExpandCollapseButtonState.Expanded;
                                        this.Invalidate();
                                    }
                                    else
                                    {
                                        treeviewNodeHits[i].TreeviewNode.ExpandCollapseButtonState = ExpandCollapseButtonState.Collapsed;
                                        this.Invalidate();
                                    }
                                    break;
                                } 
                                else
                                {
                                    if (OnMCLTreeviewNodeLabelTapped != null)
                                        OnMCLTreeviewNodeLabelTapped(this, new TreeviewNodeLabelTappedEventArgs(e, treeviewNodeHits[i].TreeviewNode));
                                }
                            }
                        }
                    }
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (TreeviewBackgroundColor != null)
            {
                canvas.FillColor = TreeviewBackgroundColor;
                canvas.FillRectangle(new Rect(0, 0, this.Width, this.Height));
            } 
            else
            {
                canvas.StrokeColor = Colors.Grey;
                canvas.DrawRectangle(0, 0, (float)this.Width, (float)this.Height);
            }
            maxTreeviewNodesHeight = 0;
            treeviewNodeHits.Clear();
            if (TreeviewNodes != null && TreeviewNodes.Length > 0)
            {
                int level = 0;
                int yOffset = 0;
                canvas.SaveState();
                canvas.ClipRectangle(0, 0, (float)this.Width, (float)this.Height);
                for (int i = 0; i < TreeviewNodes.Length; i++)
                {
                    yOffset = DrawTreeviewNode(canvas, TreeviewNodes[i], level, yOffset);
                }
                canvas.ResetState();
            }
        }

        private int DrawTreeviewNode(ICanvas canvas, TreeviewNode node, int level, int yOffset)
        {
            int levelIndent = level * PerLevelNodeLeftIndent - currentPanX;
            SizeF nodeLabelSize = canvas.GetStringSize(node.Label, TreeviewNodeLabelFont, TreeviewNodeLabelFontSize);
            Helper.SetFontAttributes(canvas, TreeviewNodeLabelFont, TreeviewNodeLabelColor, TreeviewNodeLabelFontSize);
            if (node.ChildNodes != null && node.ChildNodes.Length > 0)
            {
                int heightOffset = yOffset + (TreeviewNodeHeight - ExpandCollapseButtonWidthHeight) / 2 - currentPanY;
                if (ExpandButtonImage != null && CollapseButtonImage != null)
                {
                    Microsoft.Maui.Graphics.IImage tmpImage = node.ExpandCollapseButtonState == ExpandCollapseButtonState.Expanded ? CollapseButtonImage : ExpandButtonImage;
                    canvas.DrawImage(tmpImage, levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, ExpandCollapseButtonWidthHeight);
                }
                else
                {
                    canvas.StrokeColor = Colors.Grey;
                    canvas.DrawRectangle(levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, ExpandCollapseButtonWidthHeight);
                    canvas.DrawString(node.ExpandCollapseButtonState == ExpandCollapseButtonState.Expanded ? CollapseButtonLabel : ExpandButtonLabel,
                        levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, TreeviewNodeHeight - (TreeviewNodeHeight -
                        ExpandCollapseButtonWidthHeight), HorizontalAlignment.Center, VerticalAlignment.Center);
                }
                treeviewNodeHits.Add(new TreeviewNodeHit(new RectF(levelIndent, heightOffset, ExpandCollapseButtonWidthHeight, 
                    ExpandCollapseButtonWidthHeight), HitAreaType.ExpandCollapseButton, node));
            }
            if (node.Label != null)
            {
                int tmpWidth = levelIndent + ExpandCollapseButtonWidthHeight + SpacingBetweenButtonAndLabel;
                canvas.DrawString(node.Label, tmpWidth, yOffset - currentPanY, tmpWidth + (int)nodeLabelSize.Width, TreeviewNodeHeight, HorizontalAlignment.Left, VerticalAlignment.Center);
            }
            treeviewNodeHits.Add(new TreeviewNodeHit(new RectF(levelIndent + ExpandCollapseButtonWidthHeight + SpacingBetweenButtonAndLabel, yOffset - currentPanY,
                (float)this.Width - levelIndent - ExpandCollapseButtonWidthHeight - SpacingBetweenButtonAndLabel, TreeviewNodeHeight), HitAreaType.Label, node));
            if (maxTreeviewNodeWidth < levelIndent + ExpandCollapseButtonWidthHeight + SpacingBetweenButtonAndLabel + nodeLabelSize.Width)
                maxTreeviewNodeWidth = levelIndent + ExpandCollapseButtonWidthHeight + SpacingBetweenButtonAndLabel + (int)Math.Ceiling((double)nodeLabelSize.Width);
            yOffset += TreeviewNodeHeight;
            if (node.ChildNodes != null && node.ExpandCollapseButtonState == ExpandCollapseButtonState.Expanded)
            {
                for (int i = 0; i < node.ChildNodes.Length; i++)
                {
                    yOffset = DrawTreeviewNode(canvas, node.ChildNodes[i], level + 1, yOffset);
                }
            }
            if (yOffset > maxTreeviewNodesHeight)
                maxTreeviewNodesHeight = yOffset;
            return yOffset;
        }

        public void LoadExpandCollapseImages(Assembly assembly, string manifestResourcePathExpandImage, string manifestResourcePathCollapseImage)
        {
            using (Stream? stream = assembly.GetManifestResourceStream(manifestResourcePathExpandImage))
            {
                ExpandButtonImage = PlatformImage.FromStream(stream);
                ExpandCollapseButtonWidthHeight = (int)ExpandButtonImage.Width;
            }
            using (Stream? stream = assembly.GetManifestResourceStream(manifestResourcePathCollapseImage))
            {
                CollapseButtonImage = PlatformImage.FromStream(stream);
            }
        }
    }
}
