using PhotoDrive.Common;
using PhotoDrive.Databases;
using PhotoDrive.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IImageDatabase _db;
        private readonly IImageFileService _imageFileService;

        public bool HasSynced { get; private set; }

        public ImageService(IImageDatabase db, IImageFileService imageFileService)
        {
            _db = db;
            _imageFileService = imageFileService;
        }

        public async Task<Result> SyncDbAndFileSystemAsync()
        {
            Result<List<ImageEntity>> dbFetchResult = await GetAllAsync();
            if (!dbFetchResult.IsSuccess)
                Result.Fail(dbFetchResult.Error!);

            try
            {
                List<string> localImages = _imageFileService.GetStoredFiles();
                List<string> dbImages = dbFetchResult.Value!.Select(x => x.FilePath).ToList();
                IEnumerable<string> toDelete = localImages.Except(dbImages);

                foreach (string img in toDelete)
                    _imageFileService.Delete(img);

                HasSynced = true;
                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Fail("Failed to sync filesystem with db");
            }
        }

        public async Task<Result<ImageEntity>> CreateAsync(CreateImagePayload payload)
        {
            payload.Deconstruct(out string title, out Location location, out Stream stream, out FileInfo info);

            ImageEntity newImage = new()
            {
                Title = title,
            };

            try
            {
                string fileName = string.Concat(title, "_", newImage.Id.ToString(), info.Extension);
                string savedTo = await _imageFileService.SaveAsync(stream, fileName);

                newImage.CreatedAt = DateTime.Now;
                newImage.FilePath = savedTo;
                newImage.Latitude = location.Latitude;
                newImage.Longitude = location.Longitude;

                int rowsCreated = await _db.CreateAsync(newImage);
                if (rowsCreated != 1)
                    throw new Exception();

                return Result<ImageEntity>.Success(newImage);
            }
            catch (Exception)
            {
                return Result<ImageEntity>.Fail("Failed to add image");
            }
        }

        public async Task<Result<List<ImageEntity>>> GetAllAsync()
        {
            try
            {
                List<ImageEntity> images = await _db.GetAllAsync();
                return Result<List<ImageEntity>>.Success(images);
            }
            catch (Exception)
            {
                return Result<List<ImageEntity>>.Fail("Failed to load images");
            }
        }

        public async Task<Result<ImageEntity>> GetByIdAsync(Guid id)
        {
            try
            {
                ImageEntity? image = await _db.GetByIdAsync(id);
                if (image == null)
                    throw new Exception();

                return Result<ImageEntity>.Success(image);
            }
            catch (Exception)
            {
                return Result<ImageEntity>.Fail("Failed to load image");
            }
        }

        public async Task<Result> UpdateAsync(ImageEntity image)
        {
            try
            {
                int rowsUpdated = await _db.UpdateAsync(image);
                if (rowsUpdated != 1)
                    throw new Exception();

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Fail("Failed to update image");
            }
        }

        public async Task<Result> DeleteAsync(ImageEntity image)
        {
            try
            {
                int rowsDeleted = await _db.DeleteAsync(image);
                if (rowsDeleted != 1)
                    throw new Exception();

                _imageFileService.Delete(image.FilePath);

                return Result.Success();
            }
            catch (Exception)
            {
                return Result.Fail("Failed to delete image");
            }
        }
    }
}
