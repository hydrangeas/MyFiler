﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
                    new PhysicalFileName(new Guid("2DA0C0DC-8EB9-4DF2-B224-DF57CC5671DA")),
                    123
                    )
                );
            fileDatabaseMock.Setup(x => x.GetData()).Returns(entities);

            var viewModel = new FileListViewModel(
                fileDatabaseMock.Object
                );
            viewModel.Files.Count.Is(1);
            viewModel.Files[0].LogicalFileName.Value.Is("test");
            viewModel.Files[0].PhysicalFileName.Value.Is("2DA0C0DC-8EB9-4DF2-B224-DF57CC5671DA");
            viewModel.Files[0].FileSize.Value.Is<UInt64>(123);
        }
    }
}
