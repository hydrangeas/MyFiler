using MyFiler.Domain.Entitites;
using MyFiler.Domain.ValueObjects;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyFiler.UI.FileList.ViewModels
{
    public class FileListViewModel : BindableBase
    {
        public ReadOnlyReactiveCollection<FileListViewModelFiles> FileList { get; }
            = new ReactiveCollection<FileListViewModelFiles>().ToReadOnlyReactiveCollection();
        public ObservableCollection<FileListViewModelFiles> Files { get; }
            = new ObservableCollection<FileListViewModelFiles>();

        public FileListViewModel(
            IFileDatabaseRepository fileDatabaseRepository
            )
        {
            var files = new List<FileEntity>
            {
                new FileEntity("test", new PhysicalFileName(Guid.NewGuid())),
            };

            foreach(var file in files)
            {
                Files.Add(new FileListViewModelFiles(file));
            }

            FileList = Files.ToReadOnlyReactiveCollection();
        }
    }
}
