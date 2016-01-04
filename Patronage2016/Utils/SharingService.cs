using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Patronage2016.Utils
{
    public class SharingService
    {
        private readonly DataTransferManager transferManager;

        public SharingService()
        {
            transferManager=DataTransferManager.GetForCurrentView();
            transferManager.DataRequested += OnDataRequested;
        }
        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var view = this.GetCurrentView();
            if (view == null)
                return;

            var supportSharing = view.DataContext as ISupportSharing;
            if (supportSharing == null)
                return;

            supportSharing.OnShareRequested(args.Request);
        }

        private FrameworkElement GetCurrentView()
        {
            var frame = Window.Current.Content as Frame;
            if (frame != null)
                return frame.Content as FrameworkElement;

            return Window.Current.Content as FrameworkElement;
        }
    }
}
