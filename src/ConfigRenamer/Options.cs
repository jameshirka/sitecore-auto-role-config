namespace ConfigRenamer
{
    using CommandLine;

    using ConfigRenamer.Model;

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

        #endregion
    }
}

/*
             var configSet = "8.1Upd3.csv";
            var serverRole = ServerRoleEnum.ContentManagement;
            var webRoot = @"D:\Dev\personal\ConfigRenamer\TestData\Working";
            var searchProvider = SearchProvider.Lucene;
 */