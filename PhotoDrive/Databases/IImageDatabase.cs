using PhotoDrive.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.Databases
{
    public interface IImageDatabase
    {
        Task<List<ImageEntity>> GetAllAsync();
        Task<ImageEntity?> GetByIdAsync(Guid id);
        Task<int> CreateAsync(ImageEntity image);
        Task<int> UpdateAsync(ImageEntity image);
        Task<int> DeleteAsync(ImageEntity image);
    }
}
