using CommunityToolkit.Mvvm.ComponentModel;
using PhotoDrive.Model;
using PhotoDrive.ViewModels;

namespace PhotoDrive.Pages;

public partial class EditImagePage : ContentPage
{
    private EditImagePageViewModel _viewModel;
    public EditImagePage(EditImagePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        _viewModel.InitQueryCopy();
    }
}