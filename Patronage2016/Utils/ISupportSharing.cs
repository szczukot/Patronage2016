using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace Patronage2016.Utils
{
    public interface ISupportSharing
    {
        void OnShareRequested(DataRequest dataRequest);
    }
}
