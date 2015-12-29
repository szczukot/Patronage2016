using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.BulkAccess;
using Windows.Storage.Search;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;

namespace Patronage2016.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string pathToImage;
        private BitmapImage imgSource;

        public MainViewModel()
        {
            Foo();
        }


        private async void Foo()
        {
            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            var files = await picturesFolder.GetFilesAsync();
            List<BitmapImage> bitMapsList=new List<BitmapImage>();
            List<string> Pathes=new List<string>();
            foreach (var f in files)
            {
                var stream = await f.OpenReadAsync();
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                bitMapsList.Add(bitmapImage);

                Pathes.Add(f.Path);
            }
            if (bitMapsList.Count > 1)
            {
                ImgSource = bitMapsList[0];
                PathToImage = Pathes[0];
            }
           
        }
       
        #region Methods
        
        #endregion

        #region Getters/Setters

        public BitmapImage ImgSource
        {
            get { return imgSource; }
            set
            {
                if (imgSource != value)
                {
                    imgSource = value;
                    RaisePropertyChanged();
                }
            }
        }
        
        public string PathToImage
        {
            get { return pathToImage; }
            set
            {
                if (pathToImage != value)
                {
                    pathToImage = value;
                    RaisePropertyChanged("PathToImage");
                }
            }
        }
        #endregion

    }
}
