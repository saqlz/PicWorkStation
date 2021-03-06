﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Color = System.Drawing.Color;
using Pen = System.Windows.Media.Pen;

namespace PicWorkStation
{
    [Serializable]
    public class ImageCanvas : Canvas
    {
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

            var uploadLocalFileMenu = new MenuItem();
            uploadLocalFileMenu.Header = "加载复制链接地址的图片";
            uploadLocalFileMenu.Click += btnLoadLocalFile_Click;
            this.ContextMenu.Items.Add(uploadLocalFileMenu);
        }

        private void btnLoadImage_Click(object sender, RoutedEventArgs e)
        {
            LoadImageFromClipboard();
        }

        public void LoadImageFromClipboard()
        {
            try
            {
                var bmp = ClipboardMessage.ImageFromClipboardDib();
                if (bmp != null)
                {
                    this.CanvasImageSource = bmp;
                    this.InvalidateVisual();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnLoadImageLink_Click(object sender, RoutedEventArgs e)
        {
            LoadImageFromLinkAddress();
        }

        public void LoadImageFromLinkAddress()
        {
            try
            {
                var linkAddress = ClipboardMessage.GetLinkAddressFromClipboard();
                var request = (HttpWebRequest)WebRequest.Create(linkAddress);
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var bitmap = System.Drawing.Image.FromStream(stream) as Bitmap;
                        var bitMapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                            Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        this.CanvasImageSource = bitMapSource;
                        this.InvalidateVisual();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnLoadLocalFile_Click(object sender, RoutedEventArgs e)
        {
            LoadImageFromLocalFile();
        }

        public void LoadImageFromLocalFile()
        {
            try
            {
                var localFile = ClipboardMessage.GetImageFileFromClipboard();
                using (var fileStream = new FileStream(localFile, FileMode.Open))
                {
                    var bitmap = System.Drawing.Image.FromStream(fileStream) as Bitmap;
                    var bitMapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero,
                        Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    this.CanvasImageSource = bitMapSource;
                    this.InvalidateVisual();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                if (Math.Abs(value - _scale) < 1e-5 || value < 0.1)
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

        private string _strImageBrushStyle;
        public string StrImageBrushStyle
        {
            get { return _strImageBrushStyle; }
            set
            {
                _strImageBrushStyle = value;
            }
        }

        /// <summary>
        /// 用于缩色条的宽度
        /// </summary>
        private double _ratio = 1.0;
        public double Ratio
        {
            get { return _ratio; }
            set
            {
                if (Math.Abs(value - _ratio) < 1e-5 || value < 1)
                {
                    return;
                }
                _ratio = value;
            }
        }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            dc.PushTransform(_scaleTransform);
            dc.PushTransform(_translateTransform);
            dc.DrawImage(CanvasImageSource, new Rect(0, 0, RenderSize.Width, RenderSize.Height));

            if (!string.IsNullOrEmpty(_strImageBrushStyle))
            {
                double horizontalMiddlePos = RenderSize.Width / 2;
                double verticalMiddlePos = RenderSize.Height / 2;

                //水平方向
                ImageBrush imgBrush = BrushHelper.GetImageBrush(_strImageBrushStyle);
                imgBrush.RelativeTransform = new RotateTransform(90, 0.5, 0.5);
                Pen horizontalPen = new Pen(imgBrush, imgBrush.ImageSource.Width / _ratio);
                dc.DrawLine(horizontalPen, new System.Windows.Point(horizontalMiddlePos, verticalMiddlePos), 
                            new System.Windows.Point(imgBrush.ImageSource.Height + horizontalMiddlePos, verticalMiddlePos));
                dc.DrawLine(horizontalPen, new System.Windows.Point(horizontalMiddlePos, verticalMiddlePos),
                            new System.Windows.Point(horizontalMiddlePos - imgBrush.ImageSource.Height, verticalMiddlePos));

                //垂直方向
                ImageBrush verticalImgBrush = imgBrush.CloneCurrentValue();
                Pen verticalPen = new Pen(verticalImgBrush, imgBrush.ImageSource.Width / _ratio);
                verticalImgBrush.RelativeTransform = new RotateTransform(0, 0.5, 0.5);
                dc.DrawLine(verticalPen, new System.Windows.Point(horizontalMiddlePos, verticalMiddlePos),
                           new System.Windows.Point(horizontalMiddlePos, verticalMiddlePos + imgBrush.ImageSource.Height));
                dc.DrawLine(verticalPen, new System.Windows.Point(horizontalMiddlePos, verticalMiddlePos),
                           new System.Windows.Point(horizontalMiddlePos, verticalMiddlePos - imgBrush.ImageSource.Height));
            }
            base.OnRender(dc);
        }

    }
}
