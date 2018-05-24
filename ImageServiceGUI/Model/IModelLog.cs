using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    interface IModelLog : INotifyPropertyChanged
    {
        ObservableCollection<LogInfo> LogsList { get; set; }
        void GetLogsFromService();
    }
 }
