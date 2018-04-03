using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail 
        //private static Regex r = new Regex(":");

        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;
        }

        public string AddFile(string path, out bool result)
        {
            string imageName = Path.GetFileName(path);
            //  check if the file is exist
            if (!File.Exists(path))
            {
                result = false;
                return "Error: the file is not exist";
            }

            //  get the date of the image
            DateTime date = File.GetCreationTime(path);
            string year = date.Year.ToString();
            string month = date.Month.ToString();

            //  create directory's
            Directory.CreateDirectory(Path.Combine(this.m_OutputFolder,year,month));
            Directory.CreateDirectory(Path.Combine(this.m_OutputFolder, "thumbnails", year, month));


            string destPath = string.Format("{0}\\{1}\\{2}\\{3}",m_OutputFolder,year, month , imageName);
            if (!File.Exists(destPath))
            {
                File.Copy(path, destPath);
            }

            //  check if thumbnail file is exist
            string destThumbPath = string.Format("{0}\\{1}\\{2}\\{3}\\{4}", m_OutputFolder, "thumbnails", year, month, imageName);
            if (!File.Exists(destThumbPath))
            {
                //  get the thumbnail of the image
                using (Image image = Image.FromFile(path))
                using (Image thumbnail = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero))
                {
                    //thumbnail.Save(destPath);
                    thumbnail.Save(destThumbPath);
                }
            }
            result = true;
            return destPath;
        }

        /*
        private DateTime getImageDate(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }*/

        #endregion

    }
}
