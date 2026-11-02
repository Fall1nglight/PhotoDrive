using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using PhotoDrive.Databases;
using PhotoDrive.Pages;
using PhotoDrive.Services;
using PhotoDrive.Services.ImageService;
using PhotoDrive.ViewModels;

namespace PhotoDrive
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IImageDatabase, SQLiteImageDatabase>();
            builder.Services.AddSingleton<IImageFileService, ImageFileService>();
            builder.Services.AddSingleton<IImageService, ImageService>();
            builder.Services.AddSingleton<MainPageViewModel>();
            builder.Services.AddSingleton<AddImagePageViewModel>();
            builder.Services.AddSingleton<AddImagePage>();
            builder.Services.AddTransient<EditImagePageViewModel>();
            builder.Services.AddTransient<EditImagePage>();
            builder.Services.AddTransient<ViewImagePageViewModel>();
            builder.Services.AddTransient<ViewImagePage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
