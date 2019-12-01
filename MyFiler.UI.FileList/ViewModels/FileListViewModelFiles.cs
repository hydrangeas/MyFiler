using MyFiler.Domain.Entitites;
using MyFiler.Domain.ValueObjects;
using Prism.Mvvm;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Reactive.Disposables;

namespace MyFiler.UI.FileList.ViewModels
{
    public class FileListViewModelFiles: BindableBase
    {
        public ReadOnlyReactivePropertySlim<FileEntity> FileDetail { get; }
            = new ReactivePropertySlim<FileEntity>().ToReadOnlyReactivePropertySlim();
        public ReadOnlyReactivePropertySlim<string> LogicalFileName { get; }
            = new ReactivePropertySlim<string>().ToReadOnlyReactivePropertySlim();
        public ReadOnlyReactivePropertySlim<string> PhysicalFileName { get; }
            = new ReactivePropertySlim<string>().ToReadOnlyReactivePropertySlim();
        public ReadOnlyReactivePropertySlim<string> FileSize { get; }
            = new ReactivePropertySlim<string>().ToReadOnlyReactivePropertySlim();
        public ReadOnlyReactivePropertySlim<string> Comment { get; }

        private CompositeDisposable disposables = new CompositeDisposable();

        public FileListViewModelFiles(FileEntity fileEntity)
        {
            FileDetail
                = new ReactivePropertySlim<FileEntity>(fileEntity)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(disposables);

            LogicalFileName = FileDetail.Value.LogicalFileName
                .ToReadOnlyReactivePropertySlim<string>()
                .AddTo(disposables);

            PhysicalFileName
                = new ReactivePropertySlim<string>(fileEntity.PhysicalFileName.Value.DisplayValue)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(disposables);

            FileSize
                = new ReactivePropertySlim<string>(fileEntity.FileSize.Value.DisplayValueWithUnit)
                .ToReadOnlyReactivePropertySlim()
                .AddTo(disposables);

            Comment
                = new ReactivePropertySlim<string>(fileEntity.Comment.Value.DisplayValueWithoutNewline)
                .ToReadOnlyReactivePropertySlim<string>()
                .AddTo(disposables);
        }
    }
}
