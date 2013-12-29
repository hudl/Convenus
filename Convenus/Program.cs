using System;
using System.IO;
using System.Linq;
using System.Security;
using Nancy.Hosting.Self;
using Newtonsoft.Json;

namespace Convenus
{
    class Program
    {
        internal static StartupOptions Options { get; set; }
        private static NancyHost _currentHost;

        static void Main(string[] args)
        {
            //CODE NAME Convenus (kon-ven-ous) (Latin for meeting)

            //use http://nssm.cc/ (External/nssm.exe) to install as a service
            Run(args);

            Console.WriteLine("Running Nancy Host. Listening on port {0}...", Options.Port);
            Console.ReadLine();

            Stop();

        }
        public static void Run(string[] args)
        {
            var options = new StartupOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                //throw error
            }
            if (!string.IsNullOrWhiteSpace(options.ConfigFileName))
            {
                LoadConfigFile(options);
            }

            SecureString password = ParsePassword(options);
            options.Password = null;

            Console.WriteLine("Starting Exchange service...");
            ExchangeServiceHelper.Init(options.UserName, password);

            Options = options;
            _currentHost = new NancyHost(new Uri("http://" + options.Uri + ":" + options.Port));
            _currentHost.Start(); 
        }
        public static void Stop()
        {
            if (_currentHost != null)
            {
                _currentHost.Stop();                
            }
        }

        private static SecureString ParsePassword(StartupOptions options)
        {
            var ss = new SecureString();
            if (!string.IsNullOrEmpty(options.Password))
            {
                foreach (char c in options.Password)
                {
                    ss.AppendChar(c);
                }
            }
            ss.MakeReadOnly();
            return ss;
        }

        private static void LoadConfigFile(StartupOptions options)
        {
            var configText = File.ReadAllText(options.ConfigFileName);
            var newOptions = JsonConvert.DeserializeObject<StartupOptions>(configText);

            //now merge the new options into the existing (config file options always win)
            if (newOptions.CompanyName != null)
            {
                options.CompanyName = newOptions.CompanyName;
            }
            if (newOptions.Password != null)
            {
                options.Password = newOptions.Password;
            }
            if (newOptions.Port != null)
            {
                options.Port = newOptions.Port;
            }
            if (newOptions.RefreshInterval != null)
            {
                options.RefreshInterval = newOptions.RefreshInterval;
            }
            if (newOptions.RequireAuth != null)
            {
                options.RequireAuth = newOptions.RequireAuth;
            }
            if (newOptions.AuthPin != null)
            {
                options.AuthPin = newOptions.AuthPin;
            }
            if (newOptions.Uri != null)
            {
                options.Uri = newOptions.Uri;
            }
            if (newOptions.AllowCors != null)
            {
                options.AllowCors = newOptions.AllowCors;
            }
            if (newOptions.UserName != null)
            {
                options.UserName = newOptions.UserName;
            }

            options.ConfigFileName = null; //make sure this isn't available later
        }
    }
}
