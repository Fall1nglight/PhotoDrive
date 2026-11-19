using PhotoDrive.Common;
using PhotoDrive.Pages;

namespace PhotoDrive
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(PageNames.AddImagePage, typeof(AddImagePage));
            Routing.RegisterRoute(PageNames.EditImagePage, typeof(EditImagePage));
            Routing.RegisterRoute(PageNames.ViewImagePage, typeof(ViewImagePage));
        }
    }
}
