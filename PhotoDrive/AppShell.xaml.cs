using PhotoDrive.Pages;

namespace PhotoDrive
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("add-image", typeof(AddImagePage));
            Routing.RegisterRoute("edit-image", typeof(EditImagePage));
            Routing.RegisterRoute("view-image", typeof(ViewImagePage));
        }
    }
}
