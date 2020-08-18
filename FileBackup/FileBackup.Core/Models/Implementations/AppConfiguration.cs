using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileBackup.Core.Types;
using Newtonsoft.Json;

namespace FileBackup.Core.Models.Implementations
{
    public class AppConfiguration
    {
        [JsonRequired]
        public LoggingLevel LoggingLevel { get; }
        [JsonRequired]
        public List<String> OriginalPath { get; }
        [JsonRequired]
        public String TargetPath { get; }

        [JsonConstructor]
        private AppConfiguration(LoggingLevel loggingLevel, String[] originalPath, String targetPath)
        {
            LoggingLevel = loggingLevel;
            OriginalPath = originalPath.ToList();
            TargetPath = targetPath;
        }

        public static AppConfiguration FromJson(String configFilePath)
        {
            try
            {
                String configFileText = File.ReadAllText(configFilePath);
                return JsonConvert.DeserializeObject<AppConfiguration>(configFileText);
            }
            catch (JsonSerializationException)
            {
                throw FileBackupException.Config.UnableToReadFormFile(configFilePath);
            }
            catch (Exception)
            {
                throw FileBackupException.Config.WrongConfigFormat(configFilePath);
            }
        }
    }
}