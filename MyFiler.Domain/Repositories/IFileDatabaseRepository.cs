using MyFiler.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFiler.Domain.ValueObjects
{
    public interface IFileDatabaseRepository
    {
        IReadOnlyList<FileEntity> GetData();
        void Save(FileEntity fileEntity);
        void Delete(FileEntity fileEntity);
    }
}
