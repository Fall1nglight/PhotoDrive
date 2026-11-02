using PhotoDrive.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.Services
{
    public interface IImageFileService
    {
        Task<string> SaveAsync(Stream imgStream, string filename);
        List<string> GetStoredFiles();
        void Delete(string filePath);
    }
}
