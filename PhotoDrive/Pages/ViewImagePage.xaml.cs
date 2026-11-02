using PhotoDrive.ViewModels;

namespace PhotoDrive.Pages;

public partial class ViewImagePage : ContentPage
{
    private readonly ViewImagePageViewModel _viewModel;

    public ViewImagePage(ViewImagePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}