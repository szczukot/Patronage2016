using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Email;
using Windows.Devices.Enumeration;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Media.Capture;
using Windows.Media.Effects;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.BulkAccess;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Patronage2016.Messages;
using Patronage2016.Navigation;
using Patronage2016.Utils;

namespace Patronage2016.ViewModel
{
    public class MainViewModel : ViewModelBase, ISupportSharing
    {
        private List<string> pathes;
        private string pathToImage;
        private BitmapImage imgSource;
        private List<BitmapImage> bitMapsList;
        private List<BitmapImage> thumbnailsList;
        private int currentBitmapIndex;
        private List<ImageProperties> imageProps;
        private string informationsTextBlock;
        private Navigation.NavigationService _nav = new Navigation.NavigationService();
        List<StorageFile> streamList = new List<StorageFile>();

        public MainViewModel()
        {
            Messenger.Default.Register<CurrentIndexMessage>(this, x=>HandleIndexMessage(x.CurrentIndex));
            currentBitmapIndex = 0;
            bitMapsList = new List<BitmapImage>();
            thumbnailsList = new List<BitmapImage>();
            imageProps = new List<ImageProperties>();
            pathes = new List<string>();
            SwitchImageCommand = new RelayCommand(SwitchImage);
            PhotoCommand = new RelayCommand(Photo);
            GoToPhotosListCommand = new RelayCommand(GoToPhotosList);
            ShareCommand = new RelayCommand(Share);
            Initialize();
        }




        #region Methods
        private async void Initialize()
        {

            StorageFolder picturesFolder = KnownFolders.PicturesLibrary;
            var files = await picturesFolder.GetFilesAsync(CommonFileQuery.OrderByDate);

            foreach (var f in files)
            {
                var stream = await f.OpenReadAsync();
                streamList.Add((StorageFile)f);
                
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                bitMapsList.Add(bitmapImage);
                pathes.Add(f.Path);
                ImageProperties temp = await GetImageProperties((StorageFile) f);
                //thumbnails
                var thumbnailImage = new BitmapImage();
                thumbnailImage.SetSource(await f.GetThumbnailAsync(ThumbnailMode.PicturesView));
                thumbnailsList.Add(thumbnailImage);
            }
            if (bitMapsList.Count >= 1)
            {
                ImgSource = bitMapsList[currentBitmapIndex];
                InformationsTextBlock = generateInformations(imageProps[currentBitmapIndex]);
                PathToImage = pathes[currentBitmapIndex];
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
            get { return informationsTextBlock; }
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
            int? lastIndex = bitMapsList.Count - 1;
            if (bitMapsList.Count > 1)
            {
                if (currentBitmapIndex < lastIndex)
                    currentBitmapIndex++;
                else if (currentBitmapIndex == lastIndex)
                    currentBitmapIndex = 0;
                ImgSource = bitMapsList[currentBitmapIndex];
                InformationsTextBlock = generateInformations(imageProps[currentBitmapIndex]);
                PathToImage = pathes[currentBitmapIndex];
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
                infos += "\nHeight:\t" + props.Height;
            DateTimeOffset? tempDate = props.DateTaken;
            if (tempDate != null)
                infos += "\nDate taken:\t" + Convert.ToString(props.DateTaken);
            if (props.Latitude != null)
                infos += "\nLatitude:\t" + Convert.ToString(props.Latitude);
            if (props.Longitude != null)
                infos += "\nLongitude:\t" + Convert.ToString(props.Longitude);

            return infos;
        }

        public ICommand PhotoCommand { get; set; }

        private async void Photo()
        {

            var devices =
                await
                    Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(
                        Windows.Devices.Enumeration.DeviceClass.VideoCapture);
            if (devices.Count < 1)
            {
                MessageDialog dialogbox = new MessageDialog("There is no camera devices available", "Error");
                await dialogbox.ShowAsync();
                return;
            }
            try
            {
                CameraCaptureUI dialog = new CameraCaptureUI();

                StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);
                if (file != null)
                {
                    string photoName = GenerateFileName("Photo") + ".png";
                    var fileCopy = await file.CopyAsync(KnownFolders.CameraRoll, photoName,NameCollisionOption.GenerateUniqueName);
                    
                    if (bitMapsList.Count > 0)
                        bitMapsList.Clear();
                    if (imageProps.Count > 0)
                        imageProps.Clear();
                    if (pathes.Count > 0)
                        pathes.Clear();
                    if (thumbnailsList.Count > 0)
                        thumbnailsList.Clear();
                    if (streamList.Count > 0)
                        streamList.Clear();

                    await Task.Delay(TimeSpan.FromSeconds(1));
                    Initialize();
                }

            }
            catch (Exception ex)
            {
                MessageDialog dialogbox = new MessageDialog(ex.Message, "Error");
                await dialogbox.ShowAsync();
                bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-webcam"));
            }




        }
        public ICommand GoToPhotosListCommand { get; set; }

        private void GoToPhotosList()
        {
            _nav.Navigate(typeof (View.PhotosListView));
            Messenger.Default.Send<PassedData>(new PassedData(thumbnailsList,pathes));
        }
        public string GenerateFileName(string context)
        {
            return context + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString("N");
        }

        private void HandleIndexMessage(int index)
        {
            currentBitmapIndex = index;
            ImgSource = bitMapsList[currentBitmapIndex];
            InformationsTextBlock = generateInformations(imageProps[currentBitmapIndex]);
            PathToImage = pathes[currentBitmapIndex];
        }
        public ICommand ShareCommand { get; set; }
        private void Share()
        {
            DataTransferManager.ShowShareUI();
        }

        #endregion

        public void OnShareRequested(DataRequest dataRequest)
        {
            List<IStorageItem> storageList = new List<IStorageItem>();
            storageList.Add(streamList[currentBitmapIndex]);

            dataRequest.Data.Properties.Title = "Shared from Application";
            dataRequest.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(streamList[currentBitmapIndex]);
            dataRequest.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(streamList[currentBitmapIndex]));
            dataRequest.Data.SetStorageItems(storageList);
        }
    }


}

