using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ImageService.Infrastructure
{
    public class LogList
    {
        public ObservableCollection<LogInfo>LogsList { get; set; }

        public LogList(ObservableCollection<LogInfo> logs)
        {
            this.LogsList = logs;
        }
    }
}
