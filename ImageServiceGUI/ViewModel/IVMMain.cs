using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    public interface IVMMain : INotifyPropertyChanged
    {
        bool VMConnected { get; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
