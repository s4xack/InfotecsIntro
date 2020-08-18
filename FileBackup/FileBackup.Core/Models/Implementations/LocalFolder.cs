using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileBackup.Core.Models.Abstractions;
using FileBackup.Core.Types;

namespace FileBackup.Core.Models.Implementations
{
    public class LocalFolder : IFolder
    {
        public String SourcePath { get; }

        public LocalFolder(String path)
        {
            SourcePath = path;
        }

        public void CloneInnerToFolder(IFolder targetFolder)
        {
            if (!IsExists())
                throw FileBackupException.Folder.UnableToOpen(SourcePath);

            if (!targetFolder.IsExists())
                targetFolder.Init();

            CloneInnerFilesToFolder(targetFolder);
            CloneInnerFoldersToFolder(targetFolder);
        }

        public Boolean IsExists()
        {
            return Directory.Exists(SourcePath);
        }

        public void Init()
        {
            try
            {
                Directory.CreateDirectory(SourcePath);
            }
            catch (Exception e)
            {
                throw FileBackupException.Folder.UnableToCreate(SourcePath);
            }
        }

        public void CreateStamp()
        {
            IFolder stampFolder = new LocalFolder(Path.Combine(SourcePath, "Backup_stamp_" + DateTime.Now.ToString("hh-mm-ss_dd/MM/yy")));
            stampFolder.Init();
        }

        private void CloneInnerFilesToFolder(IFolder targetFolder)
        {
            GetInnerFiles().ForEach(file => file.CloneToFolder(targetFolder));
        }

        private void CloneInnerFoldersToFolder(IFolder targetFolder)
        {
            GetInnerFolders().ForEach(folder =>
            {
                IFolder innerTargetFolder = new LocalFolder(Path.Combine(targetFolder.SourcePath, Path.GetFileName(folder.SourcePath)));
                folder.CloneInnerToFolder(innerTargetFolder);
            });
        }

        private List<IFile> GetInnerFiles()
        {
            return Directory.GetFiles(SourcePath)
                .Select(path => new LocalFile(path) as IFile)
                .ToList();
        }

        private List<IFolder> GetInnerFolders()
        {
            return Directory.GetDirectories(SourcePath)
                .Select(path => new LocalFolder(path) as IFolder)
                .ToList();
        }
    }
}