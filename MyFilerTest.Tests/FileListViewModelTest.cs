using GongSolutions.Wpf.DragDrop;
using Microsoft.Extensions.FileProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyFiler.Domain.Entitites;
using MyFiler.Domain.Repositories;
using MyFiler.Domain.ValueObjects;
using MyFiler.UI.FileList.ViewModels;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;

namespace MyFilerTest.Tests
{
    [TestClass]
    public class FileListViewModelTest
    {
        [TestMethod]
        public void 初期状態()
        {
            var fileDatabaseMock = new Mock<IFileDatabaseRepository>();
            var entities = new List<FileEntity>();
            fileDatabaseMock.Setup(x => x.GetData()).Returns(entities);

            var viewModel = new FileListViewModel(
                fileDatabaseMock.Object,
                null
                );
            viewModel.FileList.Count.Is(0);
        }

        [TestMethod]
        public void 最初からデータがある状態()
        {
            var fileDatabaseMock = new Mock<IFileDatabaseRepository>();
            var entities = new List<FileEntity>();
            entities.Add(
                new FileEntity(
                    "test",
                    new PhysicalFileName(new Guid("2DA0C0DC-8EB9-4DF2-B224-DF57CC5671DA")),
                    new FileSize(1230),
                    new Comment("This is a pen.\n")
                    )
                );
            entities.Add(
                new FileEntity(
                    "test2",
                    new PhysicalFileName(new Guid("2DA0C0DC-8EB9-4DF2-B224-DF57CC5671DB")),
                    new FileSize(1300234),
                    new Comment("This is a \r\npen.")
                    )
                );
            fileDatabaseMock.Setup(x => x.GetData()).Returns(entities);

            var viewModel = new FileListViewModel(
                fileDatabaseMock.Object,
                null
                );
            viewModel.FileList.Count.Is(2);
            viewModel.FileList[0].LogicalFileName.Value.Is("test");
            viewModel.FileList[0].PhysicalFileName.Value.Is("2DA0C0DC-8EB9-4DF2-B224-DF57CC5671DA");
            viewModel.FileList[0].FileSize.Value.Is("1.20 KB");
            viewModel.FileList[0].Comment.Value.Is("This is a pen. ");
            viewModel.FileList[1].LogicalFileName.Value.Is("test2");
            viewModel.FileList[1].PhysicalFileName.Value.Is("2DA0C0DC-8EB9-4DF2-B224-DF57CC5671DB");
            viewModel.FileList[1].FileSize.Value.Is("1.24 MB");
            viewModel.FileList[1].Comment.Value.Is("This is a  pen.");
        }
        [TestMethod]
        public void ファイルドロップ1()
        {
            var fileDropList = new DataObject();
            fileDropList.SetFileDropList(new StringCollection {
                @"C:\public\test123.stl"
            });
            var dropInfoMock = new Mock<IDropInfo>();
            dropInfoMock.Setup(x => x.Data).Returns(fileDropList);

            /*
             * Save() が呼ばれていなくても正常に完了してしまう・・
             */
            var fileDatabaseMock = new Mock<IFileDatabaseRepository>();
            fileDatabaseMock.Setup(x => x.Save(It.IsAny<FileEntity>()))
                .Callback<FileEntity>(value =>
                {
                    var entity = new FileEntity(
                        "test123.stl",
                        new PhysicalFileName(new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563")),
                        new FileSize(1230),
                        new Comment(null)
                        );
                    value.LogicalFileName.Value.Is(entity.LogicalFileName.Value);
                    value.PhysicalFileName.Value.Is(entity.PhysicalFileName.Value);
                    value.FileSize.Value.Is(entity.FileSize.Value);
                    value.Comment.Value.Is(entity.Comment.Value);

                    fileDatabaseMock.Setup(x => x.GetData()).Returns(() => new List<FileEntity>() { entity });
                });
            fileDatabaseMock.Setup(x => x.GetData()).Returns(new List<FileEntity>());

            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.Setup(x => x.Name).Returns("test123.stl");
            fileInfoMock.Setup(x => x.Length).Returns(1230);
            var fileInformationMock = new Mock<IFileRepository>();
            fileInformationMock.Setup(x=>x.GetFileInfo(It.IsAny<string>())).Returns(fileInfoMock.Object);

            var viewModelMock = new Mock<FileListViewModel>(
                fileDatabaseMock.Object,
                fileInformationMock.Object
                );
            viewModelMock.Setup(x => x.GetNewGuid()).Returns(new Guid("E93ECBD8-EB7F-4478-B99D-C1933EBA3563"));
            var viewModel = viewModelMock.Object;

            // 
            viewModel.Drop(dropInfoMock.Object);
            viewModel.FileList.Count.Is(1);
            viewModel.FileList[0].LogicalFileName.Value.Is("test123.stl");
            viewModel.FileList[0].PhysicalFileName.Value.Is("E93ECBD8-EB7F-4478-B99D-C1933EBA3563");
            viewModel.FileList[0].FileSize.Value.Is("1.20 KB");
            viewModel.FileList[0].Comment.Value.IsNull();
        }
    }
}
