using GongSolutions.Wpf.DragDrop;
using MyFiler.Domain.Entitites;
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
        IFileDatabaseRepository fileDatabase = null;

        public ReadOnlyReactiveCollection<FileListViewModelFiles> FileList { get; }
            = new ReactiveCollection<FileListViewModelFiles>().ToReadOnlyReactiveCollection();
        public ObservableCollection<FileListViewModelFiles> Files { get; }
            = new ObservableCollection<FileListViewModelFiles>();

        public FileListViewModel(
            IFileDatabaseRepository fileDatabaseRepository
            )
        {
            fileDatabase = fileDatabaseRepository;
            var files = fileDatabaseRepository.GetData();

            foreach(var file in files)
            {
                Files.Add(new FileListViewModelFiles(file));
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
                foreach (var file in dragFileList)
                {
                    //var fileInfo = filedata.GetFileInfo(file);
                    //_logger.GetLogger().Info($"[Drop] {file} is processing..");
                    //var physicalFileName = GetNewGuid();
                    //await _cadFileStorage.Upload(fileInfo, physicalFileName);
                    //_logger.GetLogger().Info($"[Drop] {file} is uploaded");
                    //_cadFileMetadata.Save(
                    //    new CadFileEntity(
                    //            fileInfo,
                    //            physicalFileName,
                    //            CadFiles.Count == 0 ? 1 : CadFiles.Max(x => x.DisplayOrder) + 1,
                    //            GetDateTime()
                    //        ));
                    //_logger.GetLogger().Info($"[Drop] {file} is registered");
                }
                //Update();
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
    }
}
