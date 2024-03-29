﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConfigRenamer.Model;
using ConfigRenamer.Extensions;
using FluentAssertions;

namespace ConfigRenamer.Services
{
    public class ConfigRenamerService : IConfigRenamerService
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

        public ConfigRenamerService(Options options)
        {
            this.serverRole = options.ServerRole;
            this.configSet = options.ConfigSet;
            this.webRoot = options.WebRoot;
            this.searchProvider = options.SearchProvider;
        }

        #endregion

        public void Rename()
        {
            var files = new ConfigFiles(this.configSet, this.serverRole);

            foreach (var fileToProcess in files.ConfigChanges)
            {

                var configFileNameTrimmed = fileToProcess.ConfigFileName.TrimEnd(ExampleFileExtension).TrimEnd(DisabledFileExtension);
                var configPath = $"{webRoot}{fileToProcess.FilePath}{configFileNameTrimmed}";

                try
                {
                    if (fileToProcess.SearchProviderUsed == SearchProvider.All || fileToProcess.SearchProviderUsed == searchProvider)
                    {
                        ProcessRename(fileToProcess.ConfigSetting, configPath);
                    }
                    else
                    {
                        RenameToDisable(configPath);
                    }
                }
                catch (Exception e)
                {
                    Log($"Failed to rename [{configPath}], Exception [{e}]");
                }
            }
        }


        private static void Log(string message)
        {
            Console.WriteLine(message);
        }



        private static void ProcessRename(Ability setting, string configPath)
        {
            switch (setting)
            {
                case Ability.Disable:
                    RenameToDisable(configPath);
                    break;
                case Ability.Enable:
                    RenameToEnable(configPath);
                    break;
            }
        }

        private static void RenameToDisable(string configPath)
        {
            var disabledPath = $"{configPath}{DisabledFileExtension}";

            if (File.Exists(configPath))
            {
                File.Move(configPath, disabledPath);
            }

            try
            {
                File.Exists(configPath).Should().BeFalse($"Enabled File Should not Exist {configPath}");
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
        }

        private static void RenameToEnable(string configPath)
        {
            var examplePath = $"{configPath}{ExampleFileExtension}";
            var disabledPath = $"{configPath}{DisabledFileExtension}";

            if (File.Exists(disabledPath))
            {
                File.Move(disabledPath, configPath);
            }
            else if (File.Exists(examplePath))
            {
                File.Move(examplePath, configPath);
            }

            try
            {
                File.Exists(configPath).Should().BeTrue($"Enabled File Should Exist {configPath}");
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
        }


    }
}
