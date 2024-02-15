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

        public class RadioButtonGroupEventArgs
        {
            public EventArgs? EventArgs { get; set; }
            public int SelectedRadioButtonIndex { get; set; }

            public RadioButtonGroupEventArgs(EventArgs? eventArgs, int selectedIndex)
            {
                EventArgs = eventArgs;
                SelectedRadioButtonIndex = selectedIndex;
            }
        }

        public MCLRadioButtonGroup()
        {
            this.Drawable = this;
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                Point? point = e.GetPosition(this);
                if (RadioButtonsCenters != null && RadioButtonsCenters.Length > 0 && Helper.PointFValueIsInRange(point, 0, this.Width, 0, this.Height))
                {
                    bool found = false;
                    for(int i=0; i < RadioButtonsCenters.Length; i++)
                    {
                        if(Helper.PointFValueIsInRange(point, RadioButtonsCenters[i].X - RadioButtonRadius, RadioButtonsCenters[i].X + RadioButtonRadius,
                            RadioButtonsCenters[i].Y - RadioButtonRadius, RadioButtonsCenters[i].Y + RadioButtonRadius))
                        {
                            SelectedRadioButtonIndex = i;
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        if (OnMCLRadioButtonGroupTapped != null)
                            OnMCLRadioButtonGroupTapped(this, new RadioButtonGroupEventArgs(e, SelectedRadioButtonIndex));
                        this.Invalidate();
                    }
                }
            };
            this.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            int offset = 0;
            RadioButtonsCenters = new PointF[Labels.Length];
            if(RadioButtonGroupBackgroundColor != null)
            {
                canvas.FillColor = RadioButtonGroupBackgroundColor;
                canvas.FillRoundedRectangle(new RectF(0, 0, (float)this.Width, (float)this.Height), RadioButtonGroupCornerRadius);
            }
            canvas.StrokeColor = RadioButtonGroupBorderColor;
            canvas.DrawRoundedRectangle(new RectF(0, 0, (float)this.Width, (float)this.Height), RadioButtonGroupCornerRadius);
            for (int i = 0; i < Labels.Length; i++)
            {
                SizeF labelSizeF = canvas.GetStringSize(Labels[i], RadioButtonGroupFont.Font, RadioButtonGroupFont.FontSize);
                Helper.SetFontAttributes(canvas, RadioButtonGroupFont);
                canvas.DrawString(Labels[i], ArrangeHorizontal ? offset : 0, ArrangeHorizontal ? 0 : offset, labelSizeF.Width + RadioButtonGroupFont.FontSize,
                    ArrangeHorizontal ? (float)this.Height : (float)(this.Height / (double)Labels.Length), RadioButtonGroupFont.HorizontalAlignment,
                    RadioButtonGroupFont.VerticalAlignment);
                canvas.StrokeColor = Colors.Grey;
                PointF radioButtonCenter = new PointF(ArrangeHorizontal ? offset + labelSizeF.Width + SpacingBetweenLabelAndRadioButton + RadioButtonRadius :
                    labelSizeF.Width + SpacingBetweenLabelAndRadioButton + RadioButtonRadius, (ArrangeHorizontal ? (float)this.Height / 2F :
                    offset + (float)(this.Height / (double)Labels.Length) / 2F));
                RadioButtonsCenters[i] = radioButtonCenter;
                canvas.DrawCircle(radioButtonCenter, RadioButtonRadius);
                if(SelectedRadioButtonIndex == i)
                {
                    canvas.FillColor = RadioButtonColor;
                    canvas.FillCircle(radioButtonCenter, RadioButtonRadius - 1);
                }
                offset += (int)(ArrangeHorizontal ? labelSizeF.Width + SpacingBetweenLabelAndRadioButton + 2 * RadioButtonRadius + SpacingBetweenRadioButtons :
                    (float)(this.Height / (double)Labels.Length));
            }
        }
    }
}
