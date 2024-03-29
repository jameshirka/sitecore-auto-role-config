namespace ConfigRenamer.Model
{
    internal class Rename
    {

        public string ConfigFileName { get; set; }

        public string ConfigType { get; set; }

        public string FilePath { get; set; }

        public string ProductName { get; set; }

        public Ability ConfigSetting { get; set; }

        public SearchProvider SearchProviderUsed { get; set; }
    }
}