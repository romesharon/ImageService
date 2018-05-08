using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageServiceGUI.Model;

namespace ImageServiceGUI.ViewModel
{
    class VMSettings : IVMSettings
    {
        private Model.IModelSettings model;

        public string OutputDirectory
        {
            get
            {
                return this.model.OutputDirectory;
            }
        }

        public string SourceName
        {
            get
            {
                return this.model.SourceName;
            }
        }
        public string LogName
        {
            get
            {
                return this.model.LogName;
            }
        }
        public int ThumbnailSize
        {
            get
            {
                return this.model.ThumbnailSize;
            }
        }

        public VMSettings()
        {
            this.model = new ModelSettings();
        }
    }
}
