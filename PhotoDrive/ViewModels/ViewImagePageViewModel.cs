using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using PhotoDrive.Messages;
using PhotoDrive.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.ViewModels
{
    [QueryProperty(nameof(ToView), "selected-image")]
    public partial class ViewImagePageViewModel : ObservableObject, IMessageHandler<StringMessage>
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasBeenEdited))]
        [NotifyCanExecuteChangedFor(nameof(OpenMapCommand))]
        public partial ImageEntity? ToView { get; set; }

        public bool HasBeenEdited => ToView != null && ToView.UpdatedAt != null;

        private bool CanOpenMap() => ToView != null;

        public ViewImagePageViewModel()
        {
            WeakReferenceMessenger.Default.Register(this);
        }

        [RelayCommand(CanExecute = nameof(CanOpenMap))]
        public async Task OpenMap()
        {
            try
            {
                await Map.Default.OpenAsync(ToView!.Latitude, ToView!.Longitude);
            }
            catch (Exception)
            {
                SendMessage(new StringMessage("Error", "Failed to open map"));
            }
        }

        public async void Receive(StringMessage message) => await Shell.Current.DisplayAlertAsync(message.Title, message.Value, "Ok");
        public void SendMessage(StringMessage message) => WeakReferenceMessenger.Default.Send(message);
    }
}
