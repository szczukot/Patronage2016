using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.UI.Xaml.Media.Imaging;

namespace Patronage2016.Messages
{
    public class PassedData
    {
        public List<BitmapImage> BitMapList { get; set; }
        public List<String> Pathes { get; set; }

        public PassedData(List<BitmapImage> bitmapList, List<String> pathes)
        {
            this.BitMapList = bitmapList;
            this.Pathes = pathes;
        }
    }
}
