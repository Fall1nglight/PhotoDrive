using CommunityToolkit.Mvvm.ComponentModel;
using PhotoDrive.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoDrive.ViewModels
{
    [QueryProperty(nameof(ToView), "selected-image")]
    public partial class ViewImagePageViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasBeenEdited))]
        public partial ImageEntity? ToView { get; set; }

        public bool HasBeenEdited => ToView != null && ToView.UpdatedAt != null;
    }
}
