using PhotoDrive.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.Databases
{
    public class SQLiteImageDatabase : IImageDatabase
    {
        private readonly SQLiteAsyncConnection _db;
        private Task _initTask;

        public SQLiteImageDatabase()
        {
            string path = Path.Combine(FileSystem.Current.AppDataDirectory, "images.db3");
            SQLiteOpenFlags flags = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create;

            _db = new SQLiteAsyncConnection(path, flags);

            _initTask = _db.CreateTableAsync<ImageEntity>();
        }

        public Task EnsureInitializedAsync()
        {
            return _initTask;
        }

        public async Task<int> CreateAsync(ImageEntity image)
        {
            await EnsureInitializedAsync();
            return await _db.InsertAsync(image);
        }

        public async Task<List<ImageEntity>> GetAllAsync()
        {
            await EnsureInitializedAsync();
            return await _db.Table<ImageEntity>().ToListAsync();
        }

        public async Task<ImageEntity?> GetByIdAsync(Guid id)
        {
            await EnsureInitializedAsync();
            return await _db.Table<ImageEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(ImageEntity image)
        {
            await EnsureInitializedAsync();
            return await _db.UpdateAsync(image);
        }

        public async Task<int> DeleteAsync(ImageEntity image)
        {
            await EnsureInitializedAsync();
            return await _db.DeleteAsync(image);
        }
    }
}
