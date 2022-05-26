namespace Orc.Automation.TestSync
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using Catel;
    using Catel.IoC;
    using Catel.Logging;
    using Catel.Threading;
    using CommandLine;

    internal class Program
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly string Token = @"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJqaXJhOjdiYWE5ZmQ0LTM0NDctNDI1MC1hODQ0LTQ4MjIxZDZkNjFhYSIsImNvbnRleHQiOnsiYmFzZVVybCI6Imh0dHBzOlwvXC9zZXNvbHV0aW9ucy5hdGxhc3NpYW4ubmV0IiwidXNlciI6eyJhY2NvdW50SWQiOiI1ZTY4YzM5MjEyMzhmNjBjZmU2NzA1MGMifX0sImlzcyI6ImNvbS5rYW5vYWgudGVzdC1tYW5hZ2VyIiwiZXhwIjoxNjg0OTkzOTUyLCJpYXQiOjE2NTM0NTc5NTJ9.voCGakZ7syqO7h8QcODDzYMOCVtUG3EhrFT4ao5mkjI";

        #region Methods
        private static void Main(string[] args)
        {
            InitializeLogManager();

            var commandLine = Environment.GetCommandLineArgs();
            var options = new Options();

            var serviceLocator = ServiceLocator.Default;
            var commandLineParser = serviceLocator.ResolveType<ICommandLineParser>();
            var validationContext = commandLineParser.Parse(commandLine, options);
            if (validationContext.HasErrors)
            {
                Console.WriteLine(validationContext.GetErrors().First().Message);
                Environment.Exit(1);
            }

            if (options.IsHelp)
            {
                var helpWriterService = serviceLocator.ResolveType<IHelpWriterService>();
                foreach (var helpContent in helpWriterService.GetHelp(options))
                {
                    Console.WriteLine(helpContent);
                }
            }
            
            var projectName = options.Project;
            var testResultsFilePath = options.TestResultsFilePath;

            Console.WriteLine($"Sending test results from file ({testResultsFilePath}) to project {projectName}");

            var result = HttpRequestHelper.PostTestResultsAsync(@$"https://api.zephyrscale.smartbear.com/v2/automations/executions/junit?projectKey={projectName}",
                testResultsFilePath, Token).Result;


            Console.Write(result);
        }

        private static void InitializeLogManager()
        {
            LogManager.IgnoreCatelLogging = true;
            LogManager.AddListener(new BriefConsoleLogger());
        }
        #endregion
    }
}
