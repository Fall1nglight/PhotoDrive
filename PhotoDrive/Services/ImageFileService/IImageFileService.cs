namespace PhotoDrive.Services
{
    public interface IImageFileService
    {
        Task<string> SaveAsync(Stream imgStream, string filename);
        List<string> GetStoredFiles();
        void Delete(string filePath);
    }
}
