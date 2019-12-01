using MyFiler.Domain.ValueObjects;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFiler.Domain.Entitites
{
    public sealed class FileEntity
    {
        public FileEntity(
            string logicalFileName,
            PhysicalFileName physicalFileName,
            FileSize fileSize,
            Comment comment)
        {
            LogicalFileName.Value = logicalFileName;
            PhysicalFileName.Value = physicalFileName;
            FileSize.Value = fileSize;
            Comment.Value = comment;
        }

        public ReactivePropertySlim<string> LogicalFileName { get; }
            = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<PhysicalFileName> PhysicalFileName { get; }
            = new ReactivePropertySlim<PhysicalFileName>();
        public ReactivePropertySlim<FileSize> FileSize { get; }
            = new ReactivePropertySlim<FileSize>();
        public ReactivePropertySlim<Comment> Comment { get; }
            = new ReactivePropertySlim<Comment>();
    }
}
