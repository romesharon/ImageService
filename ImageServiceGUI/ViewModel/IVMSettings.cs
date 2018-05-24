using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    interface IVMSettings : INotifyPropertyChanged
    {
        string VMOutputDirectory { get; set; }
        string VMSourceName { get;}
        string VMLogName { get;}
        int VMThumbnailSize { get;}

        ObservableCollection<string> VMHandlers { get; }
    }
}
