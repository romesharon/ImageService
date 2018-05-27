using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ImageServiceGUI.ViewModel
{
    /// <summary>
    /// Interface IVMSettings
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface IVMSettings : INotifyPropertyChanged
    {
        string VMOutputDirectory { get; set; }
        string VMSourceName { get;}
        string VMLogName { get;}
        int VMThumbnailSize { get;}

        /// <summary>
        /// Gets the vm handlers.
        /// </summary>
        /// <value>The vm handlers.</value>
        ObservableCollection<string> VMHandlers { get; }
    }
}
