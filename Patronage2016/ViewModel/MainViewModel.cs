using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Email;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.BulkAccess;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Patronage2016.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string pathToImage;
        private BitmapImage imgSource;
        private List<BitmapImage> bitMapsList;
        private int currentBitmapIndex;
        private List<ImageProperties> imageProps;
        private string informationsTextBlock;

        public MainViewModel()
        {
            currentBitmapIndex = 0;
            bitMapsList = new List<BitmapImage>();
            imageProps = new List<ImageProperties>();

            SwitchImageCommand=new RelayCommand(SwitchImage);
            Initialize();
        }




        #region Methods
        private async void Initialize()
        {

            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            var files = await picturesFolder.GetFilesAsync();

            List<string> Pathes = new List<string>();
            foreach (var f in files)
            {
                var stream = await f.OpenReadAsync();
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                bitMapsList.Add(bitmapImage);
                Pathes.Add(f.Path);

                ImageProperties temp = await GetImageProperties((StorageFile) f);
            }
            if (bitMapsList.Count >= 1)
            {
                ImgSource = bitMapsList[currentBitmapIndex];
                InformationsTextBlock = generateInformations(imageProps[currentBitmapIndex]);
            }

        }

        private async Task<ImageProperties> GetImageProperties(StorageFile imageFile)
        {
            ImageProperties props = await imageFile.Properties.GetImagePropertiesAsync();
            
            string title = props.Title;
            if (title == null)
            {
                // Format does not support, or image does not contain Title property
            }
            DateTimeOffset date = props.DateTaken;
            if (date == null)
            {
                // Format does not support, or image does not contain DateTaken property
            }

            imageProps.Add(props);
            return props;
        }
        #endregion

        #region Getters/Setters

        public string InformationsTextBlock 
        {
            get
            {
                return informationsTextBlock;
            }
            set
            {
                if (informationsTextBlock != value)
                {
                    informationsTextBlock = value;
                    RaisePropertyChanged();
                }
            }
        }
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

        #region Buttons

        public ICommand SwitchImageCommand { get; set; }

        private void SwitchImage()
        {
            int? lastIndex = bitMapsList.Count-1;
            if (bitMapsList.Count > 1)
            {
                if (currentBitmapIndex < lastIndex)
                    currentBitmapIndex++;
                else if (currentBitmapIndex == lastIndex)
                    currentBitmapIndex = 0;
                ImgSource = bitMapsList[currentBitmapIndex];
                InformationsTextBlock = generateInformations(imageProps[currentBitmapIndex]);
            }
        }


        private string generateInformations(ImageProperties props)
        {
            string infos = "PICTURE INFORMATIONS: ";
            if (!string.IsNullOrEmpty(props.CameraManufacturer))
                infos += "\nCameraManufacturer:\t" + props.CameraManufacturer;
            if (!string.IsNullOrEmpty(props.CameraModel))
                infos += "\nCameraModel:\t" + props.CameraModel;
            if (!string.IsNullOrEmpty(props.Title))
                infos += "\nTitle:\t" + props.Title;
            if (props.Width != 0)
                infos += "\nWidth:\t" + props.Width;
            if (props.Height != 0)
                infos += "\nHeight:\t"+props.Height;
            DateTimeOffset? tempDate = props.DateTaken;
            if (tempDate != null)
                infos += "\nDate taken:\t" + Convert.ToString(props.DateTaken);
            if (props.Latitude != null)
                infos += "\nLatitude:\t" + Convert.ToString(props.Latitude);
            if (props.Longitude != null)
                infos += "\nLongitude:\t" + Convert.ToString(props.Longitude);


            return infos;
        }
        #endregion
    }
}
