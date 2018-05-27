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
    /// <summary>
    /// Interface IModelLog
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface IModelLog : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the logs list.
        /// </summary>
        /// <value>The logs list.</value>
        ObservableCollection<LogInfo> LogsList { get; set; }
        /// <summary>
        /// Gets the logs from service.
        /// </summary>
        void GetLogsFromService();
    }
 }
