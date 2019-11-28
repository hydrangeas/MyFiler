﻿using MyFiler.Domain.ValueObjects;
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
            UInt64 fileSize)
        {
            LogicalFileName.Value = logicalFileName;
            PhygicalFileName.Value = physicalFileName;
            FileSize.Value = fileSize;
        }

        public ReactivePropertySlim<string> LogicalFileName { get; }
            = new ReactivePropertySlim<string>();
        public ReactivePropertySlim<PhysicalFileName> PhygicalFileName { get; }
            = new ReactivePropertySlim<PhysicalFileName>();
        public ReactivePropertySlim<UInt64> FileSize { get; }
            = new ReactivePropertySlim<UInt64>();
    }
}
