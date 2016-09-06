﻿using ConfigRenamer.Services;

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
        public static void Main(string[] args)
        {
            var options = new Options();
            CommandLine.Parser.Default.ParseArgumentsStrict(args, options, () => { throw new Exception("Invalid arguments"); });

            var renameService = new ConfigRenamerService(options);
            renameService.Rename();

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

    }
}