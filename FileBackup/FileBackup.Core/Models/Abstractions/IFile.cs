using System;

namespace FileBackup.Core.Models.Abstractions
{
    public interface IFile : ISource
    {
        void CloneToFolder(IFolder targetFolder);
    }
}