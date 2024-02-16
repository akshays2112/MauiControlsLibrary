using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MauiControlsLibrary
{
    public class MCLImageCheckbox : MCLCheckbox
    {
        public new string? CheckboxCheckedText { get; }
        public new Helper.StandardFontPropterties? CheckboxFont { get; }
        public new Color? CheckboxColor { get; }
        public new Color? CheckboxTappedColor { get; }
        public new double? CheckboxCornerRadius { get; }
        public Microsoft.Maui.Graphics.IImage? CheckboxCheckedImage { get; set; }
        public Microsoft.Maui.Graphics.IImage? CheckboxUncheckedImage { get; set; }
        public bool StrecthCheckboxImage { get; set; } = false;

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            if (IsChecked && CheckboxCheckedImage != null)
                canvas.DrawImage(CheckboxCheckedImage, 0, 0, StrecthCheckboxImage ? (float)this.Width : CheckboxCheckedImage.Width,
                    StrecthCheckboxImage ? (float)this.Height : CheckboxCheckedImage.Height);
            else if(CheckboxUncheckedImage != null)
                canvas.DrawImage(CheckboxUncheckedImage, 0, 0, StrecthCheckboxImage ? (float)this.Width : CheckboxUncheckedImage.Width, 
                    StrecthCheckboxImage ? (float)this.Height : CheckboxUncheckedImage.Height);
        }

        public void LoadCheckboxImages(Assembly assembly, string? manifestResourcePathCheckboxUnchecked,
            string? manifestResourcePathCheckboxCheckedImage)
        {
            CheckboxUncheckedImage = Helper.LoadImage(assembly, manifestResourcePathCheckboxUnchecked);
            CheckboxCheckedImage = Helper.LoadImage(assembly, manifestResourcePathCheckboxCheckedImage);
        }
    }
}
