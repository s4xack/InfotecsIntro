using System;
using System.IO;
using FileBackup.Core.Models.Abstractions;
using FileBackup.Core.Types;

namespace FileBackup.Core.Models.Implementations
{
    public class LocalFile : IFile
    {
        public String SourcePath { get; }

        public LocalFile(String path)
        {
            SourcePath = path;
        }

        private Boolean IsAbleToRead()
        {
            FileStream readStream = File.OpenRead(SourcePath);
            return readStream.CanRead;
        }

        public void CloneToFolder(IFolder targetFolder)
        {
            if (!IsAbleToRead())
                throw FileBackupException.File.UnableToRead(SourcePath);

            FileStream originalStream = File.OpenRead(SourcePath);
            String targetPath = Path.Combine(targetFolder.SourcePath, Path.GetFileName(SourcePath));

            try
            {
                FileStream targetStream = File.OpenWrite(targetPath);
                originalStream.CopyTo(targetStream);
            }
            catch (Exception)
            {
                throw FileBackupException.File.UnableToWrite(targetPath);
            }
        }
    }
}