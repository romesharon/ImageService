using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace ImageService.Infrastructure
{
    public class Info
    {
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public int ThumbnailSize { get; set; }
        public List<string> Handlers { get; set; }

        public Info(string outputDir, string sourceName, List<string> handlers, string logName, int thumbnailSize)
        {
            this.OutputDir = outputDir;
            this.SourceName = sourceName;
            this.Handlers = handlers;
            this.LogName = logName;
            this.ThumbnailSize = thumbnailSize;
        }
    }
}
