using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ImageService.Infrastructure
{
    /// <summary>
    /// Class LogList.
    /// </summary>
    public class LogList
    {
        /// <summary>
        /// Gets or sets the logs list.
        /// </summary>
        /// <value>The logs list.</value>
        public ObservableCollection<LogInfo>LogsList { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogList"/> class.
        /// </summary>
        /// <param name="logs">The logs.</param>
        public LogList(ObservableCollection<LogInfo> logs)
        {
            this.LogsList = logs;
        }
    }
}
