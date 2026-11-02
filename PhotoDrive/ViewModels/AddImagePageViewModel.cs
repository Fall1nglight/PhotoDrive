using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PhotoDrive.Common;
using PhotoDrive.Databases;
using PhotoDrive.Messages;
using PhotoDrive.Model;
using PhotoDrive.Services;
using System.ComponentModel;

namespace PhotoDrive.ViewModels
{
    public partial class AddImagePageViewModel : ObservableObject, IMessageHandler<StringMessage>
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(AddPhotoCommand))]
        public partial string Title { get; set; } = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(PreviewPhoto))]
        [NotifyPropertyChangedFor(nameof(IsPreviewReady))]
        [NotifyCanExecuteChangedFor(nameof(AddPhotoCommand))]
        public partial FileResult? CapturedPhoto { get; set; }

        public ImageSource? PreviewPhoto => CapturedPhoto == null ? null : ImageSource.FromFile(CapturedPhoto.FullPath);

        public bool IsPreviewReady => PreviewPhoto != null;

        private readonly IImageService _imageService;

        public AddImagePageViewModel(IImageService imageService)
        {
            _imageService = imageService;
            WeakReferenceMessenger.Default.Register(this);
        }

        [RelayCommand]
        public async Task CapturePhotoAsync()
        {
            try
            {
                CapturedPhoto = await MediaPicker.Default.CapturePhotoAsync();
            }
            catch (Exception)
            {
                SendMessage(new StringMessage("Error", "Failed to capture photo"));
            }
        }

        public bool CanAddPhoto()
        {
            if (CapturedPhoto == null)
                return false;

            if (PreviewPhoto == null)
                return false;

            if (string.IsNullOrWhiteSpace(Title))
                return false;

            return true;
        }

        [RelayCommand(CanExecute = nameof(CanAddPhoto))]
        public async Task AddPhotoAsync()
        {
            Location? location = null;

            try
            {
                location = await Geolocation.Default.GetLocationAsync();
                if (location == null)
                    throw new Exception();
            }
            catch (Exception)
            {
                SendMessage(new StringMessage("Error", "Failed to get geolocation data"));
                return;
            }

            await using Stream stream = await CapturedPhoto!.OpenReadAsync();
            FileInfo info = new(CapturedPhoto!.FullPath);
            CreateImagePayload payload = new(Title, location, stream, info);

            Result<ImageEntity> result = await _imageService.CreateAsync(payload);
            if (!result.IsSuccess)
            {
                SendMessage(new StringMessage("Error", result.Error!));
                return;
            }

            Reset();
            SendMessage(new StringMessage("Info", "Image saved successfully"));
        }

        private void Reset()
        {
            Title = string.Empty;
            CapturedPhoto = null;
        }

        public async void Receive(StringMessage message) => await Shell.Current.DisplayAlertAsync(message.Title, message.Value, "Ok");
        public void SendMessage(StringMessage message) => WeakReferenceMessenger.Default.Send(message);
    }
}
