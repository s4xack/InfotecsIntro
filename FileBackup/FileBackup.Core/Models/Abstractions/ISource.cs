using System;

namespace FileBackup.Core.Models.Abstractions
{
    public interface ISource
    {
        public String SourcePath { get; }
    }
}