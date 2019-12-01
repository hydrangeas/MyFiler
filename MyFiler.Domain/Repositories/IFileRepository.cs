using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFiler.Domain.Repositories
{
    public interface IFileRepository
    {
        IFileInfo GetFileInfo(string path);
    }
}
