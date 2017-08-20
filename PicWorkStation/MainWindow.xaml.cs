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

            //Combobox Item
            var styleFileAndColor = new List<StyleFileAndColor>();
            styleFileAndColor.AddRange(BrushHelper.GetImageBrushFullPath().Select(brushFullPath => 
                    new StyleFileAndColor() {Image = brushFullPath, Desc = System.IO.Path.GetFileNameWithoutExtension(brushFullPath)}));
            this.ColorComBoxControl.ItemsSource = styleFileAndColor;
            this.ColorComBoxControl.SelectedIndex = 0;
            this.ColorComBoxControl.SelectionChanged += ColorComBoxControl_SelectionChanged;
        }

        /// <summary>
        /// 鼠标控制平移开始
        /// </summary>
        private Point ptMouseStart;  
        private void ImageCanvasControl_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ptMouseStart = e.GetPosition(ImageCanvasControl);
            this.Cursor = Cursors.Hand;
        }

        /// <summary>
        /// 鼠标控制平移结束
        /// </summary>
        private void ImageCanvasControl_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        /// <summary>
        /// 鼠标控制平移过程
        /// </summary>
        private void ImageCanvasControl_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this.Cursor == Cursors.Hand && e.LeftButton == MouseButtonState.Pressed)
            {
                Point ptMouse = e.GetPosition(ImageCanvasControl);
                ImageCanvasControl.OffsetX += (ptMouse.X - ptMouseStart.X) / 120;
                ImageCanvasControl.OffsetY += (ptMouse.Y - ptMouseStart.Y) / 120;
                ImageCanvasControl.InvalidateVisual();
            }
        }

        /// <summary>
        /// 鼠标控制缩放
        /// </summary>
        private void ImageCanvasControl_OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var changedValue = Convert.ToDouble(e.Delta);
            ImageCanvasControl.Scale += (changedValue / 1200);
            ImageCanvasControl.InvalidateVisual();
        }

        /// <summary>
        /// 按钮控制图像重置
        /// </summary>
        private void ImageReset_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvasControl.Scale = 1.0;
            ImageCanvasControl.OffsetX = 0.0;
            ImageCanvasControl.OffsetY = 0.0;
            ImageCanvasControl.InvalidateVisual();
        }

        /// <summary>
        /// 加载Windows系统剪切板的图像数据
        /// </summary>
        private void LoadImageFromClipboard_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvasControl.LoadImageFromClipboard();
        }

        /// <summary>
        /// 加载Windows系统剪切板的WEB图像地址
        /// </summary>
        private void LoadImageFromLinkAddress_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvasControl.LoadImageFromLinkAddress();
        }

        /// <summary>
        /// 加载Windows系统剪切板的本地图像地址
        /// </summary>
        private void LoadImageFromLocalFile_Click(object sender, RoutedEventArgs e)
        {
            ImageCanvasControl.LoadImageFromLocalFile();
        }

        /// <summary>
        /// 监听KeyDown事件 Ctrl+V
        /// </summary>
        private void WindowControl_OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) && Keyboard.IsKeyDown(Key.V))
            {
                ImageCanvasControl.LoadImageFromClipboard();
                ImageCanvasControl.LoadImageFromLinkAddress();
                ImageCanvasControl.LoadImageFromLocalFile();
            }
        }

        /// <summary>
        /// ColorCheckboxControl_Checked 选中事件
        /// </summary>
        private void ColorCheckboxControl_Checked(object sender, RoutedEventArgs e)
        {
            if (ColorCheckboxControl.IsChecked.Value)
            {
                ImageCanvasControl.StrImageBrushStyle = (this.ColorComBoxControl.SelectedItem as StyleFileAndColor).Desc;
            }
            else
            {
                ImageCanvasControl.StrImageBrushStyle = string.Empty;
            }
            ImageCanvasControl.InvalidateVisual();
        }

        /// <summary>
        /// ColorComBoxControl Combobox切换事件
        /// </summary>
        private void ColorComBoxControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColorCheckboxControl.IsChecked.Value)
            {
                ImageCanvasControl.StrImageBrushStyle = (this.ColorComBoxControl.SelectedItem as StyleFileAndColor).Desc;
                ImageCanvasControl.InvalidateVisual();
            }
        }

        private void ColoSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ColorCheckboxControl.IsChecked.Value)
            {
                ImageCanvasControl.Ratio = e.OldValue;
                ImageCanvasControl.InvalidateVisual();
            }
        }
    }

    public class StyleFileAndColor
    {
        public string Image { get; set; }
        public string Desc { get; set; }
    }
}
