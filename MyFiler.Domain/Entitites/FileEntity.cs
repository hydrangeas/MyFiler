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
            PhysicalFileName physicalFileName)
        {
            LogicalFileName.Value = logicalFileName;
            PhygicalFileName.Value = physicalFileName;
        }

        public ReactivePropertySlim<string> LogicalFileName { get; }
            = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<PhysicalFileName> PhygicalFileName { get; }
            = new ReactivePropertySlim<PhysicalFileName>();
    }
}
