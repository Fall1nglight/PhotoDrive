using PhotoDrive.Common;
using PhotoDrive.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.Services
{
    public record CreateImagePayload(string title, Location location, Stream stream, FileInfo info);

    public interface IImageService
    {
        bool HasSynced { get; }
        Task<Result> SyncDbAndFileSystemAsync();
        Task<Result<ImageEntity>> CreateAsync(CreateImagePayload param);
        Task<Result<ImageEntity>> GetByIdAsync(Guid id);
        Task<Result<List<ImageEntity>>> GetAllAsync();
        Task<Result> UpdateAsync(ImageEntity image);
        Task<Result> DeleteAsync(ImageEntity image);
    }
}
