namespace ConfigRenamer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using ConfigRenamer.Model;

    using FluentAssertions;

    internal class Program
    {
        #region Public Methods and Operators

        public static void Main(string[] args)
        {
            var options = new Options();
            CommandLine.Parser.Default.ParseArgumentsStrict(
                args, 
                options, 
                () => { throw new Exception("Invalid arguments"); });

            var configSet = options.Version;
            var serverRole = options.ServerRole;
            var webRoot = options.WebRoot;
            var searchProvider = options.SearchProvider;

            var renames = LoadConfigSet(configSet);

            foreach (var rename in renames)
            {
                var configFileNameTrimmed = rename.ConfigFileName.TrimEnd(".example").TrimEnd(".disabled");

                var configPath = string.Format("{0}{1}{2}", webRoot, rename.FilePath, configFileNameTrimmed);
                var examplePath = string.Format("{0}{1}{2}.example", webRoot, rename.FilePath, configFileNameTrimmed);
                var disabledPath = string.Format("{0}{1}{2}.disabled", webRoot, rename.FilePath, configFileNameTrimmed);

                if (rename.SearchProviderUsed == SearchProviderEnum.All || rename.SearchProviderUsed == searchProvider)
                {
                    switch (serverRole)
                    {
                        case ServerRoleEnum.ContentManagement:
                            ProcessRename(rename.RoleContentManagement, configPath, disabledPath, examplePath);

                            break;
                        case ServerRoleEnum.ContentDelivery:
                            ProcessRename(rename.RoleContentDelivery, configPath, disabledPath, examplePath);
                            break;
                        case ServerRoleEnum.Processing:
                            ProcessRename(rename.RoleProcessing, configPath, disabledPath, examplePath);
                            break;
                        case ServerRoleEnum.CmProcessing:
                            ProcessRename(rename.RoleCmProcessing, configPath, disabledPath, examplePath);
                            break;
                        case ServerRoleEnum.Reporting:
                            ProcessRename(rename.RoleReporting, configPath, disabledPath, examplePath);
                            break;
                    }
                }
                else
                {
                    RenameToDisable(configPath, disabledPath, examplePath);
                }
            }

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        #endregion

        #region Methods

        private static List<Rename> LoadConfigSet(string configSet)
        {
            var configLines = File.ReadAllLines(string.Format("./ConfigSets/{0}.csv", configSet));

            var headerRow = configLines.First();
            var dataRows = configLines.Skip(1);

            return
                dataRows.Select(dataRow => dataRow.Split(','))
                    .Select(
                        data =>
                        new Rename
                            {
                                ProductName = data[0], 
                                FilePath = data[1].TrimStart(@"\website").TrimEnd('\\') + @"\", 
                                ConfigFileName = data[2], 
                                ConfigType = data[3], 
                                SearchProviderUsed = ParseSearchProvider(data[4]), 
                                RoleContentDelivery = ParesEnabledSetting(data[5]), 
                                RoleContentManagement = ParesEnabledSetting(data[6]), 
                                RoleProcessing = ParesEnabledSetting(data[7]), 
                                RoleCmProcessing = ParesEnabledSetting(data[8]), 
                                RoleReporting = ParesEnabledSetting(data[9]), 
                            })
                    .ToList();
        }

        private static void Log(string message)
        {
            Console.WriteLine(message);
        }

        private static SettingEnum ParesEnabledSetting(string data)
        {
            if (data.ToLowerInvariant().Contains("enable"))
            {
                return SettingEnum.Enable;
            }

            if (data.ToLowerInvariant().Contains("disable"))
            {
                return SettingEnum.Disable;
            }

            return SettingEnum.NotApplicable;
        }

        private static SearchProviderEnum ParseSearchProvider(string data)
        {
            if (data.ToLowerInvariant().Contains("lucene"))
            {
                return SearchProviderEnum.Lucene;
            }

            if (data.ToLowerInvariant().Contains("solr"))
            {
                return SearchProviderEnum.Solr;
            }

            return SearchProviderEnum.All;
        }

        private static void ProcessRename(
            SettingEnum setting, 
            string configPath, 
            string disabledPath, 
            string examplePath)
        {
            switch (setting)
            {
                case SettingEnum.Disable:
                    RenameToDisable(configPath, disabledPath, examplePath);
                    break;
                case SettingEnum.Enable:
                    RenameToEnable(disabledPath, configPath, examplePath);
                    break;
            }
        }

        private static void RenameToDisable(string configPath, string disabledPath, string examplePath)
        {
            if (File.Exists(configPath))
            {
                File.Move(configPath, disabledPath);
            }

            try
            {
                File.Exists(configPath).Should().BeFalse("Enabled File Should not Exist {0}", configPath);
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
        }

        private static void RenameToEnable(string disabledPath, string configPath, string examplePath)
        {
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
                File.Exists(configPath).Should().BeTrue("Enabled File Should Exist {0}", configPath);
            }
            catch (Exception exception)
            {
                Log(exception.Message);
            }
        }

        #endregion
    }
}