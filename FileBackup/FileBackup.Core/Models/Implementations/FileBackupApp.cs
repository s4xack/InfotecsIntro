using System;
using FileBackup.Core.Models.Abstractions;
using FileBackup.Core.Types;
using Serilog;
using Serilog.Events;

namespace FileBackup.Core.Models.Implementations
{
    public class FileBackupApp : IApp
    {
        private readonly AppConfiguration _config;

        public FileBackupApp(AppConfiguration config)
        {
            _config = config;
            ConfigureLogger();

            Log.Debug("Backup configured");
        }

        public void Run()
        {
            Log.Information("Starting backup creating.");
            try
            {
                _config.OriginalPath.ForEach(path =>
                {
                    IFolder originalFolder = new LocalFolder(path);
                    IFolder targetFolder = new LocalFolder(_config.TargetPath);

                    targetFolder.CreateStamp();
                    originalFolder.CloneInnerToFolder(targetFolder);
                });

            }
            catch (FileBackupException e)
            {
                Log.Information(e.Message);
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
        }

        private void ConfigureLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log.txt", outputTemplate: "[{Level:u3}] {Message:lj}\n")
                .MinimumLevel.Is(_config.LoggingLevel)
                .CreateLogger();
        }
    }
}