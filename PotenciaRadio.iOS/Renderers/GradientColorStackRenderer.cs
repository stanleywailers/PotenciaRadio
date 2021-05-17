using System;
using CoreAnimation;
using CoreGraphics;
using PotenciaRadio.iOS.Renderers;
using PotenciaRadio.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(GradientColorStack), typeof(GradientColorStackRenderer))]
namespace PotenciaRadio.iOS.Renderers
{
    public class GradientColorStackRenderer : VisualElementRenderer<StackLayout>
    {
        public override void Draw(CGRect rect)
        {
            base.Draw(rect);
            GradientColorStack stack = (GradientColorStack)this.Element;
            CGColor startColor = stack.StartColor.ToCGColor();
            CGColor endColor = stack.EndColor.ToCGColor();
            #region for Vertical Gradient  
            var gradientLayer = new CAGradientLayer()
            {
                StartPoint = new CGPoint(0.5, 0),
                EndPoint = new CGPoint(0.5, 1)
            };
            #endregion
            #region for Horizontal Gradient  
            //var gradientLayer = new CAGradientLayer()
            //{
            //    StartPoint = new CGPoint(0, 0.5),
            //    EndPoint = new CGPoint(1, 0.5)
            //};
            #endregion
            gradientLayer.Frame = rect;
            gradientLayer.Colors = new CGColor[] {
                startColor,
                endColor
            };
            NativeView.Layer.InsertSublayer(gradientLayer, 0);
        }
    }
}
