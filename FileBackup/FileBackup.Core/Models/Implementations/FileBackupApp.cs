using System;
using FileBackup.Core.Models.Abstractions;

namespace FileBackup.Core.Models.Implementations
{
    public class FileBackupApp : IApp
    {
        private readonly AppConfiguration _config;

        public FileBackupApp(AppConfiguration config)
        {
            _config = config;
        }

        public void Run()
        {
            _config.OriginalPath.ForEach(path =>
            {
                IFolder originalFolder = new LocalFolder(path);
                IFolder targetFolder = new LocalFolder(_config.TargetPath);

                targetFolder.CreateStamp();
                originalFolder.CloneInnerToFolder(targetFolder);
            });
            
        }
    }
}