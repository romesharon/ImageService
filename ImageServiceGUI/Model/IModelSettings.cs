using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    interface IModelSettings : INotifyPropertyChanged
    {
        string OutputDirectory { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        int ThumbnailSize { get; set; }
        bool Connected { get; set; }
        ObservableCollection<string> Handlers { get; }

        void RemoveHandler(string handlerPath);
        void GetInfoFromService(); // get settings from server
    }
}
