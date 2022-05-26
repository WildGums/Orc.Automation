namespace Orc.Automation.TestSync
{
    using CommandLine;

    public class Options : ContextBase
    {
        #region Properties
        [Option("p", "project", IsMandatory = true, HelpText = "jira project name")]
        public string Project { get; set; }

        [Option("t", "test", IsMandatory = true, HelpText = "test result file")]
        public string OutputFolder { get; set; }
        #endregion
    }
}
