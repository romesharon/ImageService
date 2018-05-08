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
        string OutputDirectory { get; }
        string SourceName { get;}
        string LogName { get;}
        int ThumbnailSize { get;}
        //observable collection for the table
    }
}
