using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.Model
{
    public partial class ImageEntity : ObservableObject
    {
        [PrimaryKey]
        public Guid Id { get; init; } = Guid.NewGuid();

        [ObservableProperty]
        public partial string Title { get; set; }
        public string FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsValid() => string.IsNullOrWhiteSpace(Title) == false;

        public ImageEntity GetCopy() => (ImageEntity)MemberwiseClone();
    }
}
