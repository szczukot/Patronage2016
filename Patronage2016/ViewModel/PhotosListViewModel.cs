using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Patronage2016.Messages;
using Patronage2016.Model;
using Patronage2016.Navigation;

namespace Patronage2016.ViewModel
{
    public class PhotosListViewModel : ViewModelBase
    {
        private List<BitmapImage> bitmapList;
        private List<string> pathesList;
        private Navigation.NavigationService _nav = new Navigation.NavigationService();
        private ObservableCollection<GalleryModel> galleryCollection;


        public PhotosListViewModel()
        {

            Messenger.Default.Register<PassedData>(this,x=>HandlePassedData(x.BitMapList, x.Pathes));
            GoBackCommand = new RelayCommand(GoBack);
            SelectedItemCommand=new RelayCommand<string>(SelectedItem);
            galleryCollection=new ObservableCollection<GalleryModel>();
            
        }

        private void HandlePassedData(List<BitmapImage> argBitmap, List<string> argPathes)
        {
            if(bitmapList!=null && bitmapList.Count >0)
                bitmapList.Clear();
            if(pathesList !=null && pathesList.Count >0)
                pathesList.Clear();
            this.bitmapList = new List<BitmapImage>(argBitmap);
            this.pathesList = new List<string>(argPathes);

            if (bitmapList.Count == pathesList.Count)
            {
                if (GalleryCollection != null && GalleryCollection.Count > 0)
                    GalleryCollection.Clear();
                for (int i = 0; i < bitmapList.Count; i++)
                {
                    GalleryCollection.Add(new GalleryModel(bitmapList[i],Path.GetFileName(pathesList[i])));
                }
            }

        }

        #region Buttons
        public ICommand GoBackCommand { get; set; }

        private void GoBack()
        {
            if(_nav.CanGoBack())
                _nav.GoBack();
        }

        public ICommand SelectedItemCommand { get; set; }

        private void SelectedItem(string path)
        {
            int index = pathesList.FindIndex(x => Path.GetFileName(x) == path);
            if (index != -1)
            {
                Messenger.Default.Send<CurrentIndexMessage>(new CurrentIndexMessage(index));
                GoBack();
            }
        }

        #endregion
        #region Getters/Setters


        public ObservableCollection<GalleryModel> GalleryCollection
        {
            get
            {
                return galleryCollection;
            }
            set
            {
                if (galleryCollection != value)
                {
                    galleryCollection = value;
                    RaisePropertyChanged();
                }

            }
        }

        #endregion
    }
}
