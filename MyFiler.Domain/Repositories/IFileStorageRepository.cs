using Microsoft.Extensions.FileProviders;
using MyFiler.Domain.Entitites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MyFiler.Domain.Repositories
{
    public interface IFileStorageRepository
    {
        Task Upload(FileEntity fileEntity, IFileInfo fileInfo);
        Task Download(FileEntity fileEntity, string savePath = null);
    }
}
