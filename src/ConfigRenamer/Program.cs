using System;
using ConfigRenamer.Model;
using ConfigRenamer.Services;

namespace ConfigRenamer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var options = new Options();
            CommandLine.Parser.Default.ParseArgumentsStrict(args, options, () => { throw new Exception("Invalid arguments"); });

            if (options.Validate)
            {
                var validationService = new ValidationService(options);
                validationService.Validate();
            }
            else
            {
                var renameService = new ConfigRenamerService(options);
                renameService.Rename();

            }
        }

    }
}