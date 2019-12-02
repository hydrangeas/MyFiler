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
        public IFileStorageRepository FileStorage = null;

        public ReactiveCollection<FileListViewModelFiles> FileList { get; }
            = new ReactiveCollection<FileListViewModelFiles>();

        public FileListViewModel(
            IFileDatabaseRepository fileDatabaseRepository,
            IFileRepository fileRepository,
            IFileStorageRepository fileStorageRepository
            )
        {
            FileDatabase = fileDatabaseRepository;
            FileInformation = fileRepository;
            FileStorage = fileStorageRepository;

            Update();
        }

        private DelegateCommand<object> _DownloadCommand;
        public DelegateCommand<object> DownloadCommand =>
            _DownloadCommand ?? (_DownloadCommand = new DelegateCommand<object>(DownloadAsync));

        public async void DownloadAsync(object _fileEntity)
        {
            var fileEntity = (_fileEntity as ReadOnlyReactivePropertySlim<FileEntity>).Value;
            await FileStorage.Download(fileEntity);
        }

        private DelegateCommand<object> _DeleteCommand;
        public DelegateCommand<object> DeleteCommand =>
            _DeleteCommand ?? (_DeleteCommand = new DelegateCommand<object>(Delete));

        public void Delete(object _fileEntity)
        {
            var fileEntity = (_fileEntity as ReadOnlyReactivePropertySlim<FileEntity>).Value;
            FileDatabase.Delete(fileEntity);
            Update();
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

        public async void Drop(IDropInfo dropInfo)
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
                    var fileEntity = new FileEntity(
                            fileInfo.Name,
                            new PhysicalFileName(GetNewGuid()),
                            new FileSize((UInt64)fileInfo.Length),
                            new Comment(null)
                            );
                    await FileStorage.Upload(fileEntity, fileInfo);
                    //_logger.GetLogger().Info($"[Drop] {file} is uploaded");
                    FileDatabase.Save(fileEntity);
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
