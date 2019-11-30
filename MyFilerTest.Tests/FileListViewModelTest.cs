using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyFiler.Domain.Entitites;
using MyFiler.Domain.ValueObjects;
using MyFiler.UI.FileList.ViewModels;
using System;
using System.Collections.Generic;

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
                fileDatabaseMock.Object
                );
            viewModel.Files.Count.Is(0);
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
                    new Comment("")
                    )
                );
            entities.Add(
                new FileEntity(
                    "test2",
                    new PhysicalFileName(new Guid("2DA0C0DC-8EB9-4DF2-B224-DF57CC5671DB")),
                    new FileSize(1300234),
                    new Comment("Test Comment")
                    )
                ); ;
            fileDatabaseMock.Setup(x => x.GetData()).Returns(entities);

            var viewModel = new FileListViewModel(
                fileDatabaseMock.Object
                );
            viewModel.Files.Count.Is(2);
            viewModel.Files[0].LogicalFileName.Value.Is("test");
            viewModel.Files[0].PhysicalFileName.Value.Is("2DA0C0DC-8EB9-4DF2-B224-DF57CC5671DA");
            viewModel.Files[0].FileSize.Value.Is("1.20 KB");
            viewModel.Files[0].Comment.Value.Is("");
            viewModel.Files[1].LogicalFileName.Value.Is("test2");
            viewModel.Files[1].PhysicalFileName.Value.Is("2DA0C0DC-8EB9-4DF2-B224-DF57CC5671DB");
            viewModel.Files[1].FileSize.Value.Is("1.24 MB");
            viewModel.Files[1].Comment.Value.Is("Test Comment");

            viewModel.FileList.Count.Is(2);
            viewModel.FileList[0].FileSize.Is(viewModel.Files[0].FileSize);
            viewModel.FileList[0].LogicalFileName.Is(viewModel.Files[0].LogicalFileName);
            viewModel.FileList[0].PhysicalFileName.Is(viewModel.Files[0].PhysicalFileName);
            viewModel.FileList[0].Comment.Is(viewModel.Files[0].Comment);
            viewModel.FileList[1].LogicalFileName.Is(viewModel.Files[1].LogicalFileName);
            viewModel.FileList[1].PhysicalFileName.Is(viewModel.Files[1].PhysicalFileName);
            viewModel.FileList[1].FileSize.Is(viewModel.Files[1].FileSize);
            viewModel.FileList[1].Comment.Is(viewModel.Files[1].Comment);
        }
    }
}
