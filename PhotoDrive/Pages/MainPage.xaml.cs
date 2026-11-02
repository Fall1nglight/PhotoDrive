using Microsoft.Maui.Handlers;
using PhotoDrive.Databases;
using PhotoDrive.Services;
using PhotoDrive.ViewModels;

namespace PhotoDrive
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            await _viewModel.Init();
            await _viewModel.LoadImagesAsync();
        }

        private async void NavigateToAddImagePage(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("add-image");
        }
    }
}
