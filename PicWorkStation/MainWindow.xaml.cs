using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PicWorkStation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BitmapImage img = new BitmapImage (new Uri (@"D:\GitHub\PicWorkStation\PicWorkStation\Images\timg.jpg"));
            ImageCanvasControl.CanvasImageSource = img;
        }


        private Point ptMouseStart;  
        private void ImageCanvasControl_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ptMouseStart = e.GetPosition(ImageCanvasControl);
            this.Cursor = Cursors.Hand;
        }

        private void ImageCanvasControl_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void ImageCanvasControl_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this.Cursor == Cursors.Hand && e.LeftButton == MouseButtonState.Pressed)
            {
                Point ptMouse = e.GetPosition(ImageCanvasControl);
                ImageCanvasControl.OffsetX += (ptMouse.X - ptMouseStart.X) / 160;
                ImageCanvasControl.OffsetY += (ptMouse.Y - ptMouseStart.Y) / 160;
                ImageCanvasControl.InvalidateVisual();
            }
        }
        private void ImageCanvasControl_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var changedValue = Convert.ToDouble(e.Delta);
            ImageCanvasControl.Scale += (changedValue / 1200);
            ImageCanvasControl.InvalidateVisual();
        }

        private void ImageReset_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvasControl.Scale = 1.0;
            ImageCanvasControl.OffsetX = 0.0;
            ImageCanvasControl.OffsetY = 0.0;
            ImageCanvasControl.InvalidateVisual();
        }
    }
}
