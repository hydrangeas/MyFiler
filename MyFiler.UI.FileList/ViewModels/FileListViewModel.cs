using GongSolutions.Wpf.DragDrop;
using MyFiler.Domain.Entitites;
using MyFiler.Domain.Repositories;
using MyFiler.Domain.ValueObjects;
using Prism.Commands;
using Prism.Mvvm;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace MyFiler.UI.FileList.ViewModels
{
    public class FileListViewModel : BindableBase, IDropTarget
    {
        public IFileDatabaseRepository FileDatabase = null;
        public IFileRepository FileInformation = null;

        public ReactiveCollection<FileListViewModelFiles> FileList { get; }
            = new ReactiveCollection<FileListViewModelFiles>();

        public FileListViewModel(
            IFileDatabaseRepository fileDatabaseRepository,
            IFileRepository fileRepository
            )
        {
            FileDatabase = fileDatabaseRepository;
            FileInformation = fileRepository;

            Update();
        }

        private DelegateCommand<object> _DownloadCommand;
        public DelegateCommand<object> DownloadCommand =>
            _DownloadCommand ?? (_DownloadCommand = new DelegateCommand<object>(Download));

        void Download(object _fileEntity)
        {
            var fileEntity = (_fileEntity as ReadOnlyReactivePropertySlim<FileEntity>).Value;
        }

        public void DragOver(IDropInfo dropInfo)
        {
            var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
            foreach (var file in dragFileList)
            {
                var fileAttributes = File.GetAttributes(file);
                if (fileAttributes.HasFlag(FileAttributes.Directory))
                {
                    //_logger.GetLogger().Info($"[DragOver] {file} (Directory)");
                    dropInfo.Effects = DragDropEffects.None;
                    return;
                }
                //_logger.GetLogger().Info($"[DragOver] {file} (File)");
            }
            dropInfo.Effects = DragDropEffects.Copy;
        }

        public void Drop(IDropInfo dropInfo)
        {
            //IsBusy = true;
            try
            {
                dropInfo.Effects = DragDropEffects.All;
                var dragFileList = ((DataObject)dropInfo.Data).GetFileDropList().Cast<string>();
                foreach (var afile in dragFileList)
                {
                    var fileInfo = FileInformation.GetFileInfo(afile);
                    //_logger.GetLogger().Info($"[Drop] {file} is processing..");
                    //var physicalFileName = GetNewGuid();
                    //await _cadFileStorage.Upload(fileInfo, physicalFileName);
                    //_logger.GetLogger().Info($"[Drop] {file} is uploaded");
                    FileDatabase.Save(
                        new FileEntity(
                                fileInfo.Name,
                                new PhysicalFileName(GetNewGuid()),
                                new FileSize((UInt64)fileInfo.Length),
                                new Comment(null)
                                ));
                    //_logger.GetLogger().Info($"[Drop] {file} is registered");
                }
                Update();
            }
            catch (Exception ex)
            {
                //_logger.GetLogger().Error($"[Drop] {ex.StackTrace}");
                //await ShowErrorDialog(ex);
            }
            finally
            {
                //IsBusy = false;
            }
        }

        public void Update()
        {
            var files = FileDatabase.GetData();
            FileList.ClearOnScheduler();
            foreach (var afile in files)
            {
                FileList.AddOnScheduler(new FileListViewModelFiles(afile));
            }
        }
        public virtual Guid GetNewGuid()
        {
            return Guid.NewGuid();
        }

    }
}
