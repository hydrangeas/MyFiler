using Microsoft.Extensions.FileProviders;
using MyFiler.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyFiler.Infrastructure.LocalFile
{
    public class File : IFileRepository
    {
        public IFileInfo GetFileInfo(string path)
        {
            var physicalFileProvider = new PhysicalFileProvider(Path.GetDirectoryName(path));
            return physicalFileProvider.GetFileInfo(Path.GetFileName(path));
        }
    }
}
