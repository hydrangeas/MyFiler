using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.WindowsAzure.Storage;
using MyFiler.Domain.Entitites;
using MyFiler.Domain.Repositories;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyFiler.Infrastructure.Azure.BlobStorage
{
    public class FileStorage : IFileStorageRepository
    {
        public string _connectionString = string.Empty;
        public string ConnectionString
        {
            get
            {
                if (_connectionString == string.Empty)
                {
                    var configuration = new ConfigurationBuilder()
                        .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
#if DEBUG
                        .AddJsonFile(@"applicationsettings.debug.json")
                        .Build();
                    _connectionString = configuration.GetConnectionString("Azure.BlobStorage");
#else
                        // NEED: Create following file,
                        //       and set "copy output directory: Always"
                        .AddJsonFile(@"applicationsettings.json")
                        .Build();
                    _connectionString = configuration.GetConnectionString("Azure.BlobStorage");
#endif
                }
                return _connectionString;
            }
        }

        readonly string ContainerName = "files";

        public Task Download(FileEntity fileEntity, string savePath = null)
        {
            savePath
                = Path.Combine(
                    savePath ?? Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                    fileEntity.LogicalFileName.Value);

            return Task.Run(async () =>
            {
                var cloudStorageAcccount = CloudStorageAccount.Parse(ConnectionString);
                var cloudBlobClient = cloudStorageAcccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileEntity.PhysicalFileName.Value.DisplayValue);
                await cloudBlockBlob.DownloadToFileAsync(savePath, System.IO.FileMode.CreateNew);
            });
        }

        public Task Upload(FileEntity fileEntity, IFileInfo fileInfo)
        {
            return Task.Run(async () =>
            {
                var cloudStorageAcccount = CloudStorageAccount.Parse(ConnectionString);
                var cloudBlobClient = cloudStorageAcccount.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(ContainerName);

                await cloudBlobContainer.CreateIfNotExistsAsync();

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileEntity.PhysicalFileName.Value.DisplayValue);
                await cloudBlockBlob.UploadFromFileAsync(fileInfo.PhysicalPath);
            });
        }
    }
}
