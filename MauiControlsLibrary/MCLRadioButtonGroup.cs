namespace MauiControlsLibrary
{
    /// <summary>
    /// Represents a radio button group in the Maui Controls Library.
    /// </summary>
    public class MCLRadioButtonGroup : GraphicsView, IDrawable
    {
        /// <summary>
        /// Gets or sets the labels of the radio buttons.
        /// </summary>
        public string[] Labels { get; set; } = Array.Empty<string>();

        /// <summary>
        /// Gets or sets the font properties of the radio button group.
        /// </summary>
        public Helper.StandardFontPropterties RadioButtonGroupFont { get; set; } = new();

        /// <summary>
        /// Gets or sets a value indicating whether the radio buttons are arranged horizontally.
        /// </summary>
        public bool ArrangeHorizontal { get; set; } = true;

        /// <summary>
        /// Gets or sets the radius of the radio buttons.
        /// </summary>
        public int RadioButtonRadius { get; set; } = 7;

        /// <summary>
        /// Gets or sets the spacing between the label and the radio button.
        /// </summary>
        public int SpacingBetweenLabelAndRadioButton { get; set; } = 20;

        /// <summary>
        /// Gets or sets the spacing between the radio buttons.
        /// </summary>
        public int SpacingBetweenRadioButtons { get; set; } = 30;

        private int selectedRadioButtonIndex = -1;

        /// <summary>
        /// Gets or sets the index of the selected radio button.
        /// </summary>
        public int SelectedRadioButtonIndex { get => selectedRadioButtonIndex; set { selectedRadioButtonIndex = value; } }

        /// <summary>
        /// Gets or sets the color of the radio buttons.
        /// </summary>
        public Color RadioButtonColor { get; set; } = Colors.Green;

        /// <summary>
        /// Gets or sets the border color of the radio button group.
        /// </summary>
        public Color RadioButtonGroupBorderColor { get; set; } = Colors.Grey;

        /// <summary>
        /// Gets or sets the background color of the radio button group.
        /// </summary>
        public Color? RadioButtonGroupBackgroundColor { get; set; } = null;

        /// <summary>
        /// Gets or sets the corner radius of the radio button group.
        /// </summary>
        public int RadioButtonGroupCornerRadius { get; set; } = 5;

        /// <summary>
        /// Occurs when the radio button group is tapped.
        /// </summary>
        public event EventHandler<RadioButtonGroupEventArgs>? OnMCLRadioButtonGroupTapped;

        private PointF[]? RadioButtonsCenters;

        /// <summary>
        /// Represents the event data for the RadioButtonGroupTapped event.
        /// </summary>
        public class RadioButtonGroupEventArgs(EventArgs? eventArgs, int selectedIndex) : EventArgs
        {
            public EventArgs? EventArgs { get; set; } = eventArgs;
            public int SelectedRadioButtonIndex { get; set; } = selectedIndex;
        }

        public MCLRadioButtonGroup()
        {
            Drawable = this;
            Helper.CreateTapGestureRecognizer(TapGestureRecognizer_Tapped, GestureRecognizers);
        }

        /// <summary>
        /// Handles the Tapped event of the TapGestureRecognizer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            RadioButtonGroupTapped(0, (float)Width, 0, (float)Height, RadioButtonsCenters, RadioButtonRadius,
            ref selectedRadioButtonIndex, OnMCLRadioButtonGroupTapped, this, e, Invalidate);
        }

        /// <summary>
        /// Handles the tap event for the radio button group.
        /// </summary>
        /// <param name="x">The x-coordinate of the top-left corner of the radio button group.</param>
        /// <param name="width">The width of the radio button group.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the radio button group.</param>
        /// <param name="height">The height of the radio button group.</param>
        /// <param name="radioButtonsCenters">The centers of the radio buttons.</param>
        /// <param name="radioButtonRadius">The radius of the radio buttons.</param>
        /// <param name="selectedRadioButtonIndex">The index of the selected radio button.</param>
        /// <param name="onMCLRadioButtonGroupTapped">The event handler for the tap event.</param>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TappedEventArgs"/> instance containing the event data.</param>
        /// <param name="invalidate">The action to invalidate the radio button group.</param>
        public static void RadioButtonGroupTapped(float x, float width, float y, float height, PointF[]? radioButtonsCenters, int radioButtonRadius,
            ref int selectedRadioButtonIndex, EventHandler<RadioButtonGroupEventArgs>? onMCLRadioButtonGroupTapped, GraphicsView sender,
            TappedEventArgs e, Action invalidate)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.ArrayNotNullOrEmpty(radioButtonsCenters) && Helper.PointFValueIsInRange(point, x, width, y, height))
            {
                bool found = false;
                for (int i = 0; i < radioButtonsCenters?.Length; i++)
                {
                    if (Helper.PointFValueIsInRange(point, radioButtonsCenters[i].X - radioButtonRadius, radioButtonsCenters[i].X + radioButtonRadius,
                        radioButtonsCenters[i].Y - radioButtonRadius, radioButtonsCenters[i].Y + radioButtonRadius))
                    {
                        selectedRadioButtonIndex = i;
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    onMCLRadioButtonGroupTapped?.Invoke(sender, new RadioButtonGroupEventArgs(e, selectedRadioButtonIndex));
                    invalidate();
                }
            }
        }

        /// <summary>
        /// Draws the radio button group.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="dirtyRect">The area of the canvas that needs to be redrawn.</param>
        public virtual void Draw(ICanvas canvas, RectF dirtyRect)
        {
            int offset = 0;
            RadioButtonsCenters = new PointF[Labels.Length];
            DrawFrame(canvas, 0, 0, (float)Width, (float)Height);
            if (Helper.ArrayNotNullOrEmpty(Labels))
            {
                for (int i = 0; i < Labels.Length; i++)
                {
                    SizeF labelSizeF = canvas.GetStringSize(Labels[i], RadioButtonGroupFont.Font, RadioButtonGroupFont.FontSize);
                    DrawRadioButtonLabel(canvas, i, ArrangeHorizontal ? offset : 0, ArrangeHorizontal ? 0 : offset, labelSizeF.Width +
                        RadioButtonGroupFont.FontSize, ArrangeHorizontal ? (float)Height : (float)(Height / Labels.Length));
                    PointF radioButtonCenter = new(ArrangeHorizontal ? offset + labelSizeF.Width + SpacingBetweenLabelAndRadioButton + RadioButtonRadius :
                        labelSizeF.Width + SpacingBetweenLabelAndRadioButton + RadioButtonRadius, ArrangeHorizontal ? (float)Height / 2F :
                        offset + ((float)(Height / Labels.Length) / 2F));
                    RadioButtonsCenters[i] = radioButtonCenter;
                    DrawRadioButton(canvas, i, radioButtonCenter);
                    offset += (int)(ArrangeHorizontal ? labelSizeF.Width + SpacingBetweenLabelAndRadioButton + (2 * RadioButtonRadius) + SpacingBetweenRadioButtons :
                        (float)(Height / Labels.Length));
                }
            }
        }

        /// <summary>
        /// Draws the frame of the radio button group.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the frame.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the frame.</param>
        /// <param name="width">The width of the frame.</param>
        /// <param name="height">The height of the frame.</param>
        public virtual void DrawFrame(ICanvas canvas, float x, float y, float width, float height)
        {
            if (RadioButtonGroupBackgroundColor != null)
            {
                canvas.FillColor = RadioButtonGroupBackgroundColor;
                canvas.FillRoundedRectangle(new RectF(x, y, width, height), RadioButtonGroupCornerRadius);
            }
            canvas.StrokeColor = RadioButtonGroupBorderColor;
            canvas.DrawRoundedRectangle(new RectF(x, y, width, height), RadioButtonGroupCornerRadius);
        }

        /// <summary>
        /// Draws the label of a radio button.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="labelIndex">The index of the label to draw.</param>
        /// <param name="x">The x-coordinate of the top-left corner of the label.</param>
        /// <param name="y">The y-coordinate of the top-left corner of the label.</param>
        /// <param name="width">The width of the label.</param>
        /// <param name="height">The height of the label.</param>
        public virtual void DrawRadioButtonLabel(ICanvas canvas, int labelIndex, float x, float y, float width, float height)
        {
            Helper.SetFontAttributes(canvas, RadioButtonGroupFont);
            canvas.DrawString(Labels[labelIndex], x, y, width, height, RadioButtonGroupFont.HorizontalAlignment, RadioButtonGroupFont.VerticalAlignment);
        }

        /// <summary>
        /// Draws a radio button.
        /// </summary>
        /// <param name="canvas">The canvas to draw on.</param>
        /// <param name="radioButtonGroupIndex">The index of the radio button to draw.</param>
        /// <param name="radioButtonCenter">The center of the radio button.</param>
        public virtual void DrawRadioButton(ICanvas canvas, int radioButtonGroupIndex, PointF radioButtonCenter)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawCircle(radioButtonCenter, RadioButtonRadius);
            if (SelectedRadioButtonIndex == radioButtonGroupIndex)
            {
                canvas.FillColor = RadioButtonColor;
                canvas.FillCircle(radioButtonCenter, RadioButtonRadius - 1);
            }
        }
    }
}
