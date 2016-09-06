using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConfigRenamer.Model;
using FluentAssertions;

namespace ConfigRenamer.Services
{
    public class ConfigRenamerService
    {
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
            this.configSet = options.Version;
            this.webRoot = options.WebRoot;
            this.searchProvider = options.SearchProvider;
        }

        #endregion

        public void Rename()
        {
            var files = LoadConfigSet();

            foreach (var fileToProcess in files)
            {
                var configFileNameTrimmed = fileToProcess.ConfigFileName.TrimEnd(".example").TrimEnd(".disabled");

                var configPath = $"{webRoot}{fileToProcess.FilePath}{configFileNameTrimmed}";
                if (fileToProcess.SearchProviderUsed == SearchProvider.All || fileToProcess.SearchProviderUsed == searchProvider)
                {
                    ProcessRename(fileToProcess.ConfigSetting, configPath);
                }
                else
                {
                    RenameToDisable(configPath);
                }
            }
        }

        private List<Rename> LoadConfigSet()
        {
            var configLines = File.ReadAllLines($"./ConfigSets/{configSet}.csv");

            var headerRow = configLines.First().ToLowerInvariant();
            var serverRoleIndex = headerRow.Split(',').ToList().IndexOf(serverRole.ToLowerInvariant());

            if (serverRoleIndex == -1)
            {
                throw new ApplicationException($"Could not find matching server configuration \"{serverRole}\"");
            }

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
                            ConfigSetting = ParesEnabledSetting(data[serverRoleIndex]),
                        })
                    .ToList();
        }

        private static void Log(string message)
        {
            Console.WriteLine(message);
        }

        private static Ability ParesEnabledSetting(string data)
        {
            if (data.ToLowerInvariant().Contains("enable"))
            {
                return Ability.Enable;
            }

            if (data.ToLowerInvariant().Contains("disable"))
            {
                return Ability.Disable;
            }

            return Ability.NotApplicable;
        }

        private static SearchProvider ParseSearchProvider(string data)
        {
            if (data.ToLowerInvariant().Contains("lucene"))
            {
                return SearchProvider.Lucene;
            }

            if (data.ToLowerInvariant().Contains("solr"))
            {
                return SearchProvider.Solr;
            }

            return SearchProvider.All;
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
            var disabledPath = $"{configPath}.disabled";

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

        private static void RenameToEnable(string configPath)
        {
            var examplePath = $"{configPath}.example";
            var disabledPath = $"{configPath}.disabled";

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

    }
}
