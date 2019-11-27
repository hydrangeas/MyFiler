using MyFiler.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFiler.Domain.ValueObjects
{
    public interface IFileDatabaseRepository
    {
        IReadOnlyList<FileEntity> GetData();
    }
}
