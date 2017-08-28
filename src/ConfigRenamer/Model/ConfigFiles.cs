using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ConfigRenamer.Model
{

    using ConfigRenamer.Extensions;

    internal class ConfigFiles
    {
        public List<Rename> ConfigChanges { get; }

        public ConfigFiles(string configSet, string serverRole)
        {
            var configLines = File.ReadAllLines(configSet);

            var headerRow = configLines.First().ToLowerInvariant();
            var serverRoleIndex = headerRow.Split(',').ToList().IndexOf(serverRole.ToLowerInvariant());

            if (serverRoleIndex == -1)
            {
                throw new ApplicationException($"Could not find matching server configuration \"{serverRole}\"");
            }

            var dataRows = configLines.Skip(1);

            this.ConfigChanges = 
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

        private SearchProvider ParseSearchProvider(string searchProvider)
        {
            return Enum.GetValues(typeof(SearchProvider)).Cast<SearchProvider>().FirstOrDefault(mc => searchProvider.ToLowerInvariant().Contains(mc.ToString().ToLowerInvariant()));
        }

        private static Ability ParesEnabledSetting(string enableSetting)
        {
            if (enableSetting.ToLowerInvariant().Contains("enable"))
            {
                return Ability.Enable;
            }

            if (enableSetting.ToLowerInvariant().Contains("disable"))
            {
                return Ability.Disable;
            }

            return Ability.NotApplicable;
        }

    }
}
