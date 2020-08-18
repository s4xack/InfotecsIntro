using System;
using FileBackup.Core.Models.Abstractions;
using FileBackup.Core.Models.Implementations;
using FileBackup.Core.Types;

namespace FileBackup.Core
{
    class Program
    {
        static void Main()
        {
            IApp app = new FileBackupApp(AppConfiguration.FromJson("config.json"));
            app.Run();
        }
    }
}
