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
        
        //public string SelectedItem
        //{
        //    set
        //    {
        //        this.SelectedItem = value;
        //        this.NotifyPropertyChanged("SelectedItem");
        //    }
        //    get
        //    {
        //        if (this.SelectedItem != null)
        //        {
        //            return this.SelectedItem;
        //        }
        //        return this.model.Handlers.ElementAt(0);
        //    }
        //}

        public VMSettings()
        {
            this.model = new ModelSettings();
            this.model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
            this.model.GetInfoFromService();
        }

        private void NotifyPropertyChanged(string v)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}