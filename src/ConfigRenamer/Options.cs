namespace ConfigRenamer
{
    using CommandLine;

    using ConfigRenamer.Model;

    internal class Options
    {
        #region Public Properties

        [Option('s', "SearchProvider", Required = true, HelpText = "Search Provder")]
        public SearchProviderEnum SearchProvider { get; set; }

        [Option('r', "role", Required = true, HelpText = "Server role")]
        public ServerRoleEnum ServerRole { get; set; }

        [Option('v', "version", Required = true, HelpText = "Sitecore version")]
        public string Version { get; set; }

        [Option('w', "webroot", Required = true, HelpText = "Root of website")]
        public string WebRoot { get; set; }

        #endregion
    }
}

/*
             var configSet = "8.1Upd3.csv";
            var serverRole = ServerRoleEnum.ContentManagement;
            var webRoot = @"D:\Dev\personal\ConfigRenamer\TestData\Working";
            var searchProvider = SearchProviderEnum.Lucene;
 */