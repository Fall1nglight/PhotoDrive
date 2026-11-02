using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PhotoDrive.Common;
using PhotoDrive.Databases;
using PhotoDrive.Messages;
using PhotoDrive.Model;
using PhotoDrive.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PhotoDrive.ViewModels
{
    [QueryProperty(nameof(ToEdit), "selected-image")]
    public partial class EditImagePageViewModel : ObservableObject, IMessageHandler<StringMessage>
    {
        private readonly IImageService _imageService;

        public ImageEntity ToEdit { get; set; } = null!;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        public partial ImageEntity? SelectedImage { get; set; }

        public EditImagePageViewModel(IImageService imageService)
        {
            _imageService = imageService;
            WeakReferenceMessenger.Default.Register(this);
        }

        public void InitQueryCopy()
        {
            SelectedImage = ToEdit.GetCopy();
        }

        partial void OnSelectedImageChanged(ImageEntity? oldValue, ImageEntity? newValue)
        {
            if (oldValue != null)
                oldValue.PropertyChanged -= OnSelectedImagePropertyChanged;

            if (newValue != null)
            {
                newValue.PropertyChanged += OnSelectedImagePropertyChanged;
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        private void OnSelectedImagePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            SaveCommand.NotifyCanExecuteChanged();
        }

        public bool CanSave() => SelectedImage != null && SelectedImage.IsValid();

        [RelayCommand(CanExecute = nameof(CanSave))]
        public async Task SaveAsync()
        {
            SelectedImage!.UpdatedAt = DateTime.Now;

            Result updateResult = await _imageService.UpdateAsync(SelectedImage);

            if (updateResult.IsSuccess)
                SendMessage(new StringMessage("Info", "Successfully updated image"));
            else
                SendMessage(new StringMessage("Error", updateResult.Error!));
        }

        public async void Receive(StringMessage message) => await Shell.Current.DisplayAlertAsync(message.Title, message.Value, "Ok");
        public void SendMessage(StringMessage message) => WeakReferenceMessenger.Default.Send(message);
    }
}
