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
    public class FileListViewModel : ViewModelBase, IDropTarget
    {
        public IFileDatabaseRepository FileDatabase = null;
        public IFileRepository FileInformation = null;

        public ReadOnlyReactiveCollection<FileListViewModelFiles> FileList { get; }
            = new ReactiveCollection<FileListViewModelFiles>().ToReadOnlyReactiveCollection();
        public ObservableCollection<FileListViewModelFiles> Files { get; }
            = new ObservableCollection<FileListViewModelFiles>();

        public FileListViewModel(
            IFileDatabaseRepository fileDatabaseRepository,
            IFileRepository fileRepository
            )
        {
            FileDatabase = fileDatabaseRepository;
            FileInformation = fileRepository;

            var files = FileDatabase.GetData();
            foreach (var afile in files)
            {
                Files.Add(new FileListViewModelFiles(afile));
            }
            FileList = Files.ToReadOnlyReactiveCollection();
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
                var files = FileDatabase.GetData();
                foreach (var afile in files)
                {
                    Files.Add(new FileListViewModelFiles(afile));
                }
                //FileList = Files.ToReadOnlyReactiveCollection();
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

        public override void Update()
        {
            var files = FileDatabase.GetData();
            foreach (var afile in files)
            {
                Files.Add(new FileListViewModelFiles(afile));
            }
            //FileList = Files.ToReadOnlyReactiveCollection();
        }

    }
}
