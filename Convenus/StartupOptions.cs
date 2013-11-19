using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;
using CommandLine.Text;

namespace Convenus
{
    public class StartupOptions
    {
        [Option('f',"config-file", HelpText = "Load config values from JSON file. Username/Password or Config File required.")]
        public string ConfigFileName { get; set; }

        [Option('u', "user-name", HelpText = "Office 365 user email address. Required.")]
        public string UserName { get; set; }

        [Option('p', "password", HelpText = "Office 365 user password. Required.")]
        public string Password { get; set; }

        [Option('c', "company", HelpText = "Company Name. Default: <none>")]
        public string CompanyName { get; set; }

        [Option('r', "refresh-interval",DefaultValue = 60, HelpText = "Interval in seconds to requery Exchange data. Default: 60 sec")]
        public int? RefreshInterval { get; set; }

        [Option('a', "require-auth", DefaultValue = false, HelpText = "Is Auth required to setup a room. Default: false")]
        public bool? RequireAuth { get; set; }

        [Option('n', "auth-pin", DefaultValue = 1234, HelpText = "Numeric authentication pin for room setup. 4-6 numbers. Default: 1234")]
        public int? AuthPin { get; set; }

        [Option('o', "port", DefaultValue = 80, HelpText = "Port for the web server. Default: 80")]
        public int? Port { get; set; }

        [Option('u',"uri", DefaultValue = "localhost", HelpText = "URI to bind web server. Default: localhost")]
        public string Uri { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

    }
}
