using PhotoDrive.ViewModels;

namespace PhotoDrive.Pages;

public partial class AddImagePage : ContentPage
{
    private readonly AddImagePageViewModel _viewModel;
    public AddImagePage(AddImagePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}