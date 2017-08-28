using CommandLine;

namespace ConfigRenamer.Model
{

    public class Options
    {
        #region Public Properties

        [Option('s', "SearchProvider", Required = true, HelpText = "Search Provder")]
        public SearchProvider SearchProvider { get; set; }

        [Option('r', "role", Required = true, HelpText = "Server role")]
        public string ServerRole { get; set; }

        [Option('f', "file", Required = true, HelpText = "Sitecore config file to read")]
        public string ConfigSet { get; set; }

        [Option('w', "webroot", Required = true, HelpText = "Root of website")]
        public string WebRoot { get; set; }

        [Option('v', "validate", Required = false, HelpText = "Validates the configuration")]
        public bool Validate { get; set; }

        #endregion
    }
}
