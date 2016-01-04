using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Patronage2016.Model
{
    public class GalleryModel
    {
            public BitmapImage Image { get; set; }
            public string ImagePath { get; set; }

            public GalleryModel()
            {
                Image = null;
                ImagePath = "";
            }

            public GalleryModel(BitmapImage img, string path)
            {
                Image = img;
                ImagePath = path;
            }

        
    }
}
