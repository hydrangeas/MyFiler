using MyFiler.UI.FileList.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace MyFiler.UI.FileList
{
    public class FileListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion("FileList", typeof(Views.FileList));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}