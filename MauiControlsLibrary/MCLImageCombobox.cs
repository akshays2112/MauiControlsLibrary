using System.Reflection;

namespace MauiControlsLibrary
{
    public class MCLImageCombobox : MCLCombobox
    {
        public new string? ButtonTextForListboxCollapsed { get; }
        public new string? ButtonTextForListboxExpanded { get; }
        public new Helper.StandardFontPropterties? ButtonFont { get; }
        public new Color? ButtonColor { get; }
        public new Color? ButtonTappedColor { get; }
        public Microsoft.Maui.Graphics.IImage? ComboboxButtonNotPressedImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? ComboboxButtonPressedImage { get; set; }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.StrokeColor = Colors.Grey;
            canvas.DrawRoundedRectangle(0, 0, (float)Width, ButtonHeight, ButtonCornerRadius);
            if (SelectedItemIndex >= 0 && SelectedItemIndex < Labels.Length)
            {
                Helper.SetFontAttributes(canvas, TextboxFont);
                canvas.DrawString(Labels[SelectedItemIndex], 0, 0, (float)Width - ButtonHeight, ButtonHeight,
                    TextboxFont.HorizontalAlignment, TextboxFont.VerticalAlignment);
            }
            if (ButtonTapped && ComboboxButtonPressedImage != null)
            {
                ButtonTapped = false;
                DrawButtonImage(canvas, ComboboxButtonPressedImage);
                Invalidate();
            }
            else if(ComboboxButtonNotPressedImage != null)
                DrawButtonImage(canvas, ComboboxButtonNotPressedImage);
            if (ListboxVisible && Helper.ArrayNotNullOrEmpty(Labels))
            {
                canvas.StrokeColor = Colors.Grey;
                canvas.DrawRectangle(new Rect(0, ButtonHeight, Width, Height - ButtonHeight));
                canvas.SaveState();
                canvas.ClipRectangle(0, ButtonHeight, (float)Width, (float)Height - ButtonHeight);
                int rowStart = (int)Math.Floor(currentPanY / (decimal)RowHeight) - 1;
                rowStart = Helper.ValueResetOnBoundsCheck(rowStart, 0, Labels.Length - 1);
                for (int row = rowStart; row < Labels.Length && (row * RowHeight) - currentPanY < Height; row++)
                {
                    if (Labels[row] != null)
                    {
                        Helper.SetFontAttributes(canvas, LabelFont);
                        canvas.DrawString(Labels[row], 0, ((row + 1) * RowHeight) - currentPanY, (float)Width,
                            RowHeight, LabelFont.HorizontalAlignment, LabelFont.VerticalAlignment);
                    }
                }
                canvas.ResetState();
            }
        }

        private void DrawButtonImage(ICanvas canvas, Microsoft.Maui.Graphics.IImage image)
        {
            canvas.DrawImage(image, (float)Width - image.Width, 0, image.Width, image.Height);
        }

        public void LoadComboboxButtonImages(Assembly assembly, string? manifestResourcePathComboboxButtonImageNotPressed,
            string? manifestResourcePathComboboxButtonPressedImage)
        {
            ComboboxButtonNotPressedImage = Helper.LoadImage(assembly, manifestResourcePathComboboxButtonImageNotPressed);
            if(ComboboxButtonNotPressedImage != null)
            {
                ButtonWidth = (int)ComboboxButtonNotPressedImage.Width;
                ButtonHeight = (int)ComboboxButtonNotPressedImage.Height;
            }
            ComboboxButtonPressedImage = Helper.LoadImage(assembly, manifestResourcePathComboboxButtonPressedImage);
        }
    }
}
