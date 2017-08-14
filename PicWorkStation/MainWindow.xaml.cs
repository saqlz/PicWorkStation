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


        private void ImageCanvasControl_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //ImageCanvasControl.RenderTransform();
        }

        private void ImageCanvasControl_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("ImageCanvasControl_OnMouseLeftButtonUp");
        }

        private void ImageCanvasControl_OnMouseMove(object sender, MouseEventArgs e)
        {
            Console.WriteLine("ImageCanvasControl_OnMouseMove");
        }
        private void ImageCanvasControl_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point center = e.GetPosition(ImageCanvasControl);
            var changedValue = Convert.ToDouble(e.Delta);
            ImageCanvasControl.Scale += (changedValue / 1200);
            ImageCanvasControl.InvalidateVisual();
        }
    }
}
