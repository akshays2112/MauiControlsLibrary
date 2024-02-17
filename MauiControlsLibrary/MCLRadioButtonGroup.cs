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
        public int SelectedRadioButtonIndex { get; set; } = -1;
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
            TapGestureRecognizer tapGestureRecognizer = new();
            tapGestureRecognizer.Tapped += TapGestureRecognizer_Tapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public virtual void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
        {
            Point? point = e.GetPosition(this);
            if (Helper.ArrayNotNullOrEmpty(RadioButtonsCenters) && Helper.PointFValueIsInRange(point, 0, Width, 0, Height))
            {
                bool found = false;
                for (int i = 0; i < RadioButtonsCenters.Length; i++)
                {
                    if (Helper.PointFValueIsInRange(point, RadioButtonsCenters[i].X - RadioButtonRadius, RadioButtonsCenters[i].X + RadioButtonRadius,
                        RadioButtonsCenters[i].Y - RadioButtonRadius, RadioButtonsCenters[i].Y + RadioButtonRadius))
                    {
                        SelectedRadioButtonIndex = i;
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    OnMCLRadioButtonGroupTapped?.Invoke(this, new RadioButtonGroupEventArgs(e, SelectedRadioButtonIndex));
                    Invalidate();
                }
            }
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
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
