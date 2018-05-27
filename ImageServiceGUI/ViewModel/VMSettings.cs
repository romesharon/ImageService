using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.ViewModel
{
    class VMSettings : IVMSettings
    {
        private IModelSettings model;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand RemoveCommand { get; private set; }
        private string selectedItem;
     
        public string VMOutputDirectory
        {
            get
            {
                return this.model.OutputDirectory;
            }
            set
            {
                this.model.OutputDirectory = value;
            }
        }
        public string VMSourceName
        {
            get
            {
                return this.model.SourceName;
            }
        }
        public string VMLogName
        {
            get
            {
                return this.model.LogName;
            }
        }
        public int VMThumbnailSize
        {
            get
            {
                return this.model.ThumbnailSize;
            }
        }
        public ObservableCollection<string> VMHandlers
        {
            get
            {
                return this.model.Handlers;
            }
        }
        public string SelectedItem
        {
            set
            {
                this.selectedItem = value;
                this.NotifyPropertyChanged("SelectedItem");
            }
            get
            {
                return this.selectedItem;
            }
        }

        public VMSettings()
        {
            this.model = new ModelSettings();
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
            RemoveCommand = new DelegateCommand<object>(this.RemoveItem, this.IsSelected);
            this.model.GetInfoFromService();
        }

        private void RemoveItem(object obj)
        {
            this.model.RemoveHandler(this.selectedItem);
        }

        private bool IsSelected(object obj)
        {
            if (selectedItem == null)
            {
                return false;
            }
            return true;
        }

        private void NotifyPropertyChanged(string v)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
            var command = this.RemoveCommand as DelegateCommand<object>;
            command.RaiseCanExecuteChange();
        }
    }
}