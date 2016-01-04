using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Patronage2016.Utils;

namespace Patronage2016.ViewModel
{
    public class ViewModelLocator
    {/// <summary>
     /// Initializes a new instance of the ViewModelLocator class.
     /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<PhotosListViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }


        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public PhotosListViewModel PhotosList
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PhotosListViewModel>();
                
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
        public static void RegisterSharingService()
        {
            SimpleIoc.Default.Register<SharingService>(createInstanceImmediately: true);
        }
    }
}
