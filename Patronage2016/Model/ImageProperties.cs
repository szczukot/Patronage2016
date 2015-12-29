using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace Patronage2016.Model
{
    public class ImageProperties
    {
        public string CameraManufacturer { get; set; }
        public string CameraModel { get; set; }
        public DateTimeOffset DateTaken { get; set; }
        public uint Height { get; set; }
        public uint Width { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Title { get; set; }

        public ImageProperties()
        {
            
        }
        public ImageProperties(DateTimeOffset date, string camManufacturer="", string cModel="",  uint height=0, uint width=0, double latitude=0, double longitude=0, string title="" )
        {
            this.DateTaken = date;
            this.CameraManufacturer = camManufacturer;
            this.CameraModel = cModel;
            this.Height = height;
            this.Width = width;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Title = title;
        }
        public ImageProperties(string camManufacturer = "", string cModel = "", uint height = 0, uint width = 0, double latitude = 0, double longitude = 0, string title = "")
        {
            this.CameraManufacturer = camManufacturer;
            this.CameraModel = cModel;
            this.Height = height;
            this.Width = width;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Title = title;
        }
    }
}
