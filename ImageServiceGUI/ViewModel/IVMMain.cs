using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.ViewModel
{
    /// <summary>
    /// Interface IVMMain
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public interface IVMMain : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets a value indicating whether [vm connected].
        /// </summary>
        /// <value><c>true</c> if [vm connected]; otherwise, <c>false</c>.</value>
        bool VMConnected { get; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
