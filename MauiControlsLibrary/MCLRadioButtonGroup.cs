namespace MauiControlsLibrary
{
    public class MCLRadioButtonGroup : GraphicsView, IDrawable
    {
        public string[] Labels { get; set; } = Array.Empty<string>();
        public Helper.StandardFontPropterties RadioButtonGroupFont { get; set; } = new();
        public bool ArrangeHorizontal { get; set; } = true;
        public int RadioButtonRadius { get; set; } = 7;
        public int SpacingBetweenLabelAndRadioButton { get; set; } = 20;
        public int SpacingBetweenRadioButtons { get; set; } = 30;
        private int selectedRadioButtonIndex = -1;
        public int SelectedRadioButtonIndex { get => selectedRadioButtonIndex; set { selectedRadioButtonIndex = value; } }
        public Color RadioButtonColor { get; set; } = Colors.Green;
        public Color RadioButtonGroupBorderColor { get; set; } = Colors.Grey;
        public Color? RadioButtonGroupBackgroundColor { get; set; } = null;
        public int RadioButtonGroupCornerRadius { get; set; } = 5;
        public event EventHandler<RadioButtonGroupEventArgs>? OnMCLRadioButtonGroupTapped;

        private PointF[]? RadioButtonsCenters;

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

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            RadioButtonGroupTapped(0, (float)Width, 0, (float)Height, RadioButtonsCenters, RadioButtonRadius,
            ref selectedRadioButtonIndex, OnMCLRadioButtonGroupTapped, this, e, Invalidate);
        }

        public static void RadioButtonGroupTapped(float x, float width, float y, float height, PointF[]? radioButtonsCenters, int radioButtonRadius,
            ref int selectedRadioButtonIndex, EventHandler<RadioButtonGroupEventArgs>? onMCLRadioButtonGroupTapped, GraphicsView sender,
            TappedEventArgs e, Action invalidate)
        {
            Point? point = e.GetPosition(sender);
            if (Helper.ArrayNotNullOrEmpty(radioButtonsCenters) && Helper.PointFValueIsInRange(point, x, width, y, height))
            {
                bool found = false;
                for (int i = 0; i < radioButtonsCenters.Length; i++)
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

        public virtual void DrawRadioButtonLabel(ICanvas canvas, int labelIndex, float x, float y, float width, float height)
        {
            Helper.SetFontAttributes(canvas, RadioButtonGroupFont);
            canvas.DrawString(Labels[labelIndex], x, y, width, height, RadioButtonGroupFont.HorizontalAlignment, RadioButtonGroupFont.VerticalAlignment);
        }

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
