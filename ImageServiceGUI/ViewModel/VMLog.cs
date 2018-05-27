using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.ViewModel
{
    public class VMLog : IVMLog
    {
        private IModelLog model;
        public event PropertyChangedEventHandler PropertyChanged;
        

        public ObservableCollection<LogInfo> VMLogsList {
            get
            {
                return this.model.LogsList;
            }
        }
        public VMLog()
        {
            this.model = new ModelLog();
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
            this.model.GetLogsFromService();
        }

        private void NotifyPropertyChanged(string v)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
