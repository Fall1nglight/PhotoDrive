using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PhotoDrive.Common;
using PhotoDrive.Messages;
using PhotoDrive.Model;
using PhotoDrive.Services;
using System.Collections.ObjectModel;

namespace PhotoDrive.ViewModels
{
    public static class MenuOptions
    {
        public const string Open = nameof(Open);
        public const string Edit = nameof(Edit);
        public const string Delete = nameof(Delete);
    }

    public partial class MainPageViewModel : ObservableObject, IMessageHandler<StringMessage>
    {
        public ObservableCollection<ImageEntity> Images { get; private set; } = [];
        private readonly IImageService _imageService;

        public MainPageViewModel(IImageService imageService)
        {
            _imageService = imageService;
        }

        public async Task Init()
        {
            if (!_imageService.HasSynced)
                await _imageService.SyncDbAndFileSystemAsync();
        }

        public async Task LoadImagesAsync()
        {
            Result<List<ImageEntity>> result = await _imageService.GetAllAsync();
            if (!result.IsSuccess)
            {
                SendMessage(new StringMessage("Error", result.Error!));
                return;
            }

            Images.Clear();
            result.Value!.ForEach(Images.Add);
        }

        [RelayCommand]
        public async Task ShowImageActionsMenu(ImageEntity image)
        {
            string action = await Shell.Current.DisplayActionSheetAsync("Menu", "Cancel", null,
                MenuOptions.Open, MenuOptions.Edit, MenuOptions.Delete);

            switch (action)
            {
                case MenuOptions.Open:
                    {
                        ShellNavigationQueryParameters param = new()
                        {
                            { "selected-image", image }
                        };

                        await Shell.Current.GoToAsync("view-image", param);
                        break;
                    }

                case MenuOptions.Edit:
                    {
                        ShellNavigationQueryParameters param = new()
                        {
                            { "selected-image", image }
                        };

                        await Shell.Current.GoToAsync("edit-image", param);
                        break;
                    }

                case MenuOptions.Delete:
                    {
                        await HandleDeleteImage(image);
                        break;
                    }

                default:
                    {
                        SendMessage(new StringMessage("Error", "Unknown menu choice"));
                        break;
                    }
            }
        }

        private async Task HandleDeleteImage(ImageEntity image)
        {
            bool confirmed = await Shell.Current.DisplayAlertAsync("Delete", "Are you sure you want to delete this picture?", "Yes", "No");
            if (!confirmed)
                return;

            Result deleteResult = await _imageService.DeleteAsync(image);
            if (!deleteResult.IsSuccess)
            {
                SendMessage(new StringMessage("Error", deleteResult.Error!));
                return;
            }

            Images.Remove(image);
        }

        public async void Receive(StringMessage message) => await Shell.Current.DisplayAlertAsync(message.Title, message.Value, "Ok");
        public void SendMessage(StringMessage message) => WeakReferenceMessenger.Default.Send(message);
    }
}
