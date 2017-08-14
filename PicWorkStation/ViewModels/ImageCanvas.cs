using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PicWorkStation
{
    public class ImageCanvas : Canvas
    {

        public ImageSource CanvasImageSource
        {
            get { return (ImageSource)GetValue(CanvasImageSourceProperty); }
            set { SetValue(CanvasImageSourceProperty, value); }
        }

        public static readonly DependencyProperty CanvasImageSourceProperty =
            DependencyProperty.Register("CanvasImageSource", typeof(ImageSource),
            typeof(ImageCanvas), new FrameworkPropertyMetadata(default(ImageSource)));


        private double _scale = 1.0;
        public double Scale
        {
            get { return _scale; }
            set
            {
                if (Math.Abs(value - _scale) < 1e-5 || value <= 1)
                {
                    return;
                }
                _scale = value;
            }
        }

        private double _offsetX = 0.0;
        public double OffsetX
        {
            get { return _offsetX; }
            set
            {
                if (Math.Abs(value - _offsetX) < 1e-5)
                {
                    return;
                }
                _offsetX = value;
            }
        }

        private double _offsetY = 0.0;
        public double OffsetY
        {
            get { return _offsetY; }
            set
            {
                if (Math.Abs(value - _offsetY) < 1e-5)
                {
                    return;
                }
                _offsetY = value;
            }
        }


        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            dc.PushTransform(new TranslateTransform(_offsetX, _offsetY));
            Rect rr = new Rect(0, 0, RenderSize.Width * _scale, RenderSize.Height * _scale);
            dc.DrawImage(CanvasImageSource, rr);
            base.OnRender(dc);
        }

    }
}
