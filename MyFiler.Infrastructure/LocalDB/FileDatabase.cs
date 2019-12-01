using Microsoft.Extensions.Configuration;
using MyFiler.Domain.Entitites;
using MyFiler.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MyFiler.Infrastructure.LocalDB
{
    public class FileDatabase : IFileDatabaseRepository
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
                    _connectionString = configuration.GetConnectionString("LocalDB");
#else
                        // NEED: Create following file,
                        //       and set "copy output directory: Always"
                        .AddJsonFile(@"applicationsettings.json")
                        .Build();
                    _connectionString = configuration.GetConnectionString("Azure.SQLDatabase");
#endif
                }
                return _connectionString;
            }
        }

        public IReadOnlyList<FileEntity> GetData()
        {
            string sql = @"
SELECT [id]
      ,[logical_file_name]
      ,[physical_file_name]
      ,[file_size]
      ,[comment]
      ,[created]
      ,[updated]
  FROM [metadata]
";

            var result = new List<FileEntity>();
            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(
                            new FileEntity(
                                Convert.ToString(reader["logical_file_name"]),
                                new PhysicalFileName(new Guid(Convert.ToString(reader["physical_file_name"]))),
                                new FileSize(Convert.ToUInt64(reader["file_size"])),
                                new Comment(Convert.ToString(reader["comment"]))
                        ));
                    }
                }
            }
            return result;
        }

        public void Save(FileEntity fileEntity)
        {
            string insert = @"
INSERT INTO metadata
(logical_file_name, physical_file_name, file_size, comment, created, updated)
VALUES
(@logical_file_name, @physical_file_name, @file_size, @comment, GETDATE(), GETDATE())
";
            string update = @"
UPDATE metadata
SET logical_file_name = @logical_file_name
   ,file_size = @file_size
   ,comment = @comment
   ,updated = GETDATE()
WHERE physical_file_name = @physical_file_name
";


            using (var connection = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(update, connection))
            {
                connection.Open();
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@logical_file_name", fileEntity.LogicalFileName.Value),
                    new SqlParameter("@physical_file_name", fileEntity.PhygicalFileName.Value.Value),
                    new SqlParameter("@file_size", (Int64) fileEntity.FileSize.Value.Value),
                    new SqlParameter("@comment", "")
                };
                command.Parameters.AddRange(parameters.ToArray());

                if (command.ExecuteNonQuery() < 1)
                {
                    command.CommandText = insert;
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}
