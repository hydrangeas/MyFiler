using MyFiler.Domain.Repositories;
using MyFiler.Domain.ValueObjects;
using MyFiler.Infrastructure.Azure.BlobStorage;
using MyFiler.Infrastructure.LocalDB;
using MyFiler.Infrastructure.LocalFile;
using MyFiler.UI.FileList;
using MyFiler.UI.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace MyFiler.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IFileDatabaseRepository>(new FileDatabase());
            containerRegistry.RegisterInstance<IFileRepository>(new File());
            containerRegistry.RegisterInstance<IFileStorageRepository>(new FileStorage());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<FileListModule>(InitializationMode.WhenAvailable);
        }
    }
}
