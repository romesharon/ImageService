using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    /// <summary>
    /// Interface IVMLog
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface IVMLog : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets the vm logs list.
        /// </summary>
        /// <value>The vm logs list.</value>
        ObservableCollection<LogInfo> VMLogsList { get;}
    }
}
