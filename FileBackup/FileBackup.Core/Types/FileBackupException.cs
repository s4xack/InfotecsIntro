using System;

namespace FileBackup.Core.Types
{
    public class FileBackupException : Exception
    {
        public FileBackupException() : base() {}
        public FileBackupException(String message) : base(message) {}
        public FileBackupException(String message, Exception exception) : base(message, exception) {}

        public static class File
        {
            public static FileBackupException UnableToRead(String filePath) => new FileBackupException($"Unable to read file with path {filePath}");
            public static FileBackupException UnableToWrite(String filePath) => new FileBackupException($"Unable to write file with path {filePath}");
        }

        public static class Folder
        {
            public static FileBackupException UnableToOpen(String folderPath) => new FileBackupException($"Unable to open folder with path {folderPath}");
            public static FileBackupException UnableToCreate(String folderPath) => new FileBackupException($"Unable to create folder with path {folderPath}");
        }

        public static class Config
        {
            public static FileBackupException UnableToReadFormFile(String configFilePath) => new FileBackupException($"Unable to read config file with path {configFilePath}");
            public static FileBackupException WrongConfigFormat(String configFilePath) => new FileBackupException($"Wrong config format in file with path {configFilePath}");
        }
    }
}