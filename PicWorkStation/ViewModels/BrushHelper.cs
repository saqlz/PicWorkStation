using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PicWorkStation
{
    public static class BrushHelper
    {
        private static readonly string FilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Images";
        private static Dictionary<string, ImageBrush> dicNameBrushes = new Dictionary<string, ImageBrush>();
        private static bool isLoaded;

        private static void LoadBrushes()
        {
            var filePaths = Directory.GetFiles(FilePath);
            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                if (!string.IsNullOrEmpty(fileName))
                {
                    var imgBrush = new ImageBrush();
                    imgBrush.ImageSource = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
                    imgBrush.Stretch = Stretch.UniformToFill;
                    dicNameBrushes.Add(fileName, imgBrush);
                }
            }
            isLoaded = true;
        }

        public static IList<string> GetImageBrushName()
        {
            var filePaths = Directory.GetFiles(FilePath);
            return filePaths.Select(Path.GetFileNameWithoutExtension).ToList();
        }

        public static IList<string> GetImageBrushFullPath()
        {
            var filePaths = Directory.GetFiles(FilePath);
            return filePaths;
        }

        public static ImageBrush GetImageBrush(string name)
        {
            if (!isLoaded)
            {
                LoadBrushes();
            }

            if (dicNameBrushes.ContainsKey(name))
            {
                return dicNameBrushes[name];
            }
            return null;
        }
    }
}
