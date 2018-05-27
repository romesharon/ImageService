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
    /// Interface IModelSettings
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    interface IModelSettings : INotifyPropertyChanged
    {
        string OutputDirectory { get; set; }
        string SourceName { get; set; }
        string LogName { get; set; }
        int ThumbnailSize { get; set; }
        bool Connected { get; set; }
        /// <summary>
        /// Gets the handlers.
        /// </summary>
        /// <value>The handlers.</value>
        ObservableCollection<string> Handlers { get; }

        /// <summary>
        /// Removes the handler.
        /// </summary>
        /// <param name="handlerPath">The handler path.</param>
        void RemoveHandler(string handlerPath);
        /// <summary>
        /// Gets the information from service.
        /// </summary>
        void GetInfoFromService(); // get settings from server
    }
}
