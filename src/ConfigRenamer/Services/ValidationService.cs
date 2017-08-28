using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConfigRenamer.Model;
using ConfigRenamer.Extensions;
using FluentAssertions;

namespace ConfigRenamer.Services
{
    public class ValidationService : IValidationService
    {
        #region Constants

        private const string DisabledFileExtension = ".disabled";

        private const string ExampleFileExtension = ".example";

        #endregion

        #region Fields

        private readonly string configSet;

        private readonly string serverRole;

        private readonly string webRoot;

        private readonly SearchProvider searchProvider;

        #endregion

        #region Constructor

        public ValidationService(Options options)
        {
            this.serverRole = options.ServerRole;
            this.configSet = options.ConfigSet;
            this.webRoot = options.WebRoot;
            this.searchProvider = options.SearchProvider;
        }

        #endregion

        public void Validate()
        {
            var files = new ConfigFiles(this.configSet, this.serverRole);

            foreach (var fileToProcess in files.ConfigChanges)
            {

                var configFileNameTrimmed = fileToProcess.ConfigFileName.TrimEnd(ExampleFileExtension)
                    .TrimEnd(DisabledFileExtension);
                var configPath = $"{webRoot}{fileToProcess.FilePath}{configFileNameTrimmed}";

                try
                {
                    if (fileToProcess.SearchProviderUsed == SearchProvider.All
                        || fileToProcess.SearchProviderUsed == searchProvider)
                    {
                        ValidateFile(fileToProcess.ConfigSetting, configPath);
                    }
                    else
                    {
                        ValidateDisabledFile(configPath);
                    }
                }
                catch (Exception e)
                {
                    Log($"Failed to check [{configPath}], Exception [{e}]");
                }
            }
        }


        private static void Log(string message)
        {
            Console.WriteLine(message);
        }



        private static void ValidateFile(Ability setting, string configPath)
        {
            switch (setting)
            {
                case Ability.Disable:
                    ValidateDisabledFile(configPath);
                    break;
                case Ability.Enable:
                    ValidateEnabledFile(configPath);
                    break;
            }
        }

        private static void ValidateDisabledFile(string configPath)
        {
            if (File.Exists(configPath))
            {
                Log($"- {configPath}{DisabledFileExtension}");
            }
        }

        private static void ValidateEnabledFile(string configPath)
        {
            var examplePath = $"{configPath}{ExampleFileExtension}";
            var disabledPath = $"{configPath}{DisabledFileExtension}";

            if (File.Exists(disabledPath) || File.Exists(examplePath))
            {
                Log($"+ {configPath}");
            }
        }


    }
}