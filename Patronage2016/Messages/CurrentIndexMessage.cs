using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Messaging;

namespace Patronage2016.Messages
{
    public class CurrentIndexMessage : MessageBase
    {
        public int CurrentIndex { get; set; }

        public CurrentIndexMessage(int currentIndex)
        {
            this.CurrentIndex = currentIndex;
        }
    }
}
