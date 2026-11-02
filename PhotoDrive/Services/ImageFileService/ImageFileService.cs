using PhotoDrive.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.Services
{
    public class ImageFileService : IImageFileService
    {
        private const string BaseFolderName = "images";
        private readonly string _basePath;

        public ImageFileService()
        {
            _basePath = Path.Combine(FileSystem.Current.AppDataDirectory, BaseFolderName);

            if (!Directory.Exists(_basePath))
                Directory.CreateDirectory(_basePath);
        }

        public async Task<string> SaveAsync(Stream imgStream, string filename)
        {
            string dest = Path.Combine(_basePath, filename);
            await using FileStream fs = File.Create(dest);
            await imgStream.CopyToAsync(fs);
            return dest;
        }

        public List<string> GetStoredFiles() => Directory.GetFiles(_basePath).ToList();

        public void Delete(string filePath) => File.Delete(filePath);
    }
}
