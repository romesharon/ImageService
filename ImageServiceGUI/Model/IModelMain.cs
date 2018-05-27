using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    /// <summary>
    /// Interface IModelMain
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface IModelMain : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="IModelMain"/> is connected.
        /// </summary>
        /// <value><c>true</c> if connected; otherwise, <c>false</c>.</value>
        bool Connected { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
