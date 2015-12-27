using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace Patronage2016.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public string MSG { get; set; }
        public MainViewModel()
        {
            MSG = "Test Message";
        }
    }
}
