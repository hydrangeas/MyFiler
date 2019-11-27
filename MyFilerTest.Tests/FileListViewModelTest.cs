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
            entities.Add(
                new FileEntity(
                    "test",
                    new PhysicalFileName(new Guid())
                    )
                );
            fileDatabaseMock.Setup(x => x.GetData()).Returns(entities);

            var viewModel = new FileListViewModel(
                fileDatabaseMock.Object
                );
            viewModel.Files.Count.Is(1);
        }
    }
}
