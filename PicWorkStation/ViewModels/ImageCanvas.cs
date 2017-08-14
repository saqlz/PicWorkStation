using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PicWorkStation
{
    [Serializable]
    public class ImageCanvas : Canvas
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private ScaleTransform _scaleTransform;
        private TranslateTransform _translateTransform;

        public ImageCanvas()
        {
            _scaleTransform = new ScaleTransform(_scale, _scale);
            _translateTransform = new TranslateTransform(_offsetX, _offsetY);
            this.ContextMenu = new ContextMenu();

            var uploadImageMenu = new MenuItem();
            uploadImageMenu.Header = "加载复制的图片";
            uploadImageMenu.Click += btnLoadImage_Click;
            this.ContextMenu.Items.Add(uploadImageMenu);

            var uploadImageLinkMenu = new MenuItem();
            uploadImageLinkMenu.Header = "加载复制链接地址的图片";
            uploadImageLinkMenu.Click += btnLoadImageLink_Click;
            this.ContextMenu.Items.Add(uploadImageLinkMenu);
        }

        private void btnLoadImageLink_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bmp = null;//new ClipboardMessage().GetBitmapSource();
            if (bmp != null)
            {
                this.CanvasImageSource = bmp;
                this.InvalidateVisual();
            }
        }

        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            var bmp = new ClipboardMessage().ImageFromClipboardDib();
            if (bmp != null)
            {
                this.CanvasImageSource = bmp;
                this.InvalidateVisual();
            }
        }

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
                if (Math.Abs(value - _scale) < 1e-5 || value < 1)
                {
                    return;
                }
                _scale = value;
                _scaleTransform.ScaleX = _scale;
                _scaleTransform.ScaleY = _scale;
                _scaleTransform.CenterX = this.ActualWidth / 2;
                _scaleTransform.CenterY = this.ActualHeight / 2;
            }
        }

        private double _offsetX = 0.0;
        public double OffsetX
        {
            get { return _offsetX; }
            set
            {
                _offsetX = value;
                _translateTransform.X = _offsetX;
            }
        }

        private double _offsetY = 0.0;
        public double OffsetY
        {
            get { return _offsetY; }
            set
            {
                _offsetY = value;
                _translateTransform.Y = _offsetY;
            }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            dc.PushTransform(_scaleTransform);
            dc.PushTransform(_translateTransform);
            dc.DrawImage(CanvasImageSource, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
            base.OnRender(dc);
        }

    }
}
