using MyFiler.Domain.Entitites;
using MyFiler.Domain.ValueObjects;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System.Reactive.Disposables;

namespace MyFiler.UI.FileList.ViewModels
{
    public class FileListViewModelFiles: BindableBase
    {
        public FileEntity targetFileEntity = null;
        public ReadOnlyReactivePropertySlim<string> LogicalFileName { get; }
            = new ReactivePropertySlim<string>().ToReadOnlyReactivePropertySlim();
        public ReadOnlyReactivePropertySlim<string> PhysicalFileName { get; }
            = new ReactivePropertySlim<string>().ToReadOnlyReactivePropertySlim();

        private CompositeDisposable disposables = new CompositeDisposable();

        public FileListViewModelFiles(FileEntity fileEntity)
        {
            targetFileEntity = fileEntity;

            LogicalFileName = targetFileEntity.LogicalFileName
                .ToReadOnlyReactivePropertySlim<string>()
                .AddTo(disposables);

            PhysicalFileName
                = new ReactivePropertySlim<string>(fileEntity.PhygicalFileName.Value.DisplayValue)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(disposables);
        }
    }
}
