namespace ConfigRenamer.Model
{
    internal class Rename
    {
        #region Public Properties

        public string ConfigFileName { get; set; }

        public string ConfigType { get; set; }

        public string FilePath { get; set; }

        public string ProductName { get; set; }

        public SettingEnum RoleCmProcessing { get; set; }

        public SettingEnum RoleContentDelivery { get; set; }

        public SettingEnum RoleContentManagement { get; set; }

        public SettingEnum RoleProcessing { get; set; }

        public SettingEnum RoleReporting { get; set; }

        public SearchProviderEnum SearchProviderUsed { get; set; }

        #endregion
    }
}