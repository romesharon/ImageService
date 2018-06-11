using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class PhotosModel
    {
        public event EventHandler Update;
        ConfigModel configModel;
        private string selectedPhoto;
        List<Photo> photos;
        string[] formats = { ".jpg", ".bmp", ".png", ".gif" };
        private string outputDir;

        public List<Photo> Photos
        {
            get
            {
                return photos;
            }
            set
            {
                photos = value;
            }
        }

        public string SelectedPhoto
        {
            get
            {
                return selectedPhoto;
            }
            set
            {
                selectedPhoto = value;
            }
        }

        public PhotosModel()
        {
            configModel = new ConfigModel();
            outputDir = configModel.OutputDir;
            Photos = new List<Photo>();
            configModel.Update += UpdateHandler;

        }

        public void CheckUpdate()
        {
            this.Photos = new List<Photo>();
            this.outputDir = configModel.OutputDir;
            GetPhotos();
        }

        private void UpdateHandler(Object sender, EventArgs args)
        {
            this.outputDir = configModel.OutputDir;
            this.Photos = new List<Photo>();
            GetPhotos();
            Update?.Invoke(this, null);
        }


        private void GetPhotos()
        {
            //this.outputDir = configModel.OutputDir;
            if (this.outputDir.Equals(""))
            {
                return;
            }

            string thumbnailPath = string.Format("{0}\\{1}", this.outputDir, "thumbnails");
            DirectoryInfo thumbnailDir = new DirectoryInfo(thumbnailPath);

            foreach (DirectoryInfo year in thumbnailDir.GetDirectories())
            {
                foreach (DirectoryInfo month in year.GetDirectories())
                {
                    foreach (FileInfo pic in month.GetFiles())
                    {
                        if (formats.Contains(pic.Extension))
                        {
                            try
                            {
                                string relPath = @"~\" + Path.GetFileName(outputDir) + pic.FullName.Replace(outputDir, string.Empty);
                                string name = pic.Name;
                                string tPath = relPath;
                                string path = relPath.Replace("Thumbnails\\", string.Empty);
                                int yearI = int.Parse(year.Name);
                                int monthI = int.Parse(month.Name);
                                Photo photo = new Photo(name, path, tPath, yearI, monthI, pic.FullName);
                                this.Photos.Add(photo);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                }
            }

        }

        public Photo PathToPhoto(string path)
        {
            foreach (Photo photo in this.Photos)
            {
                if (photo.ThumbnailPath == path)
                {
                    return photo;
                }
            }
            return null;
        }

        public void DeletePhoto()
        {
            try
            {
                string realPath = "";
                foreach(Photo photo in this.Photos)
                {
                    if (photo.ThumbnailPath.Equals(this.SelectedPhoto))
                    {
                        realPath = photo.RealPath;
                        break;
                    }
                }

                string path = realPath.Replace("thumbnails\\", string.Empty);
                File.Delete(realPath);
                File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
}