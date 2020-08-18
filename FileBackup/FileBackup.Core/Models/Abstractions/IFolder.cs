using System;

namespace FileBackup.Core.Models.Abstractions
{
    public interface IFolder : ISource
    {
        void CloneInnerToFolder(IFolder targetFolder);
        Boolean IsExists();
        void Init();
        void CreateStamp();
    }
}