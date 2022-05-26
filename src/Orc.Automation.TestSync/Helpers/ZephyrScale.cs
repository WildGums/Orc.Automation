namespace Orc.Automation.TestSync.Helpers
{
    using System.Threading.Tasks;
    using RestSharp;

    public static class ZephyrScale
    {
        private const string ExecutionsBaseUrl = @$"https://api.zephyrscale.smartbear.com/v2/automations/executions/junit";

        public static async Task<string> SendTestResultsAsync(string projectName, string resultsFilePath, string token)
        {
            var uri = $"{ExecutionsBaseUrl}?projectKey={projectName}";

            using var client = new RestClient();
            var request = new RestRequest(uri, Method.Post);
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddFile("file", resultsFilePath);
            var response = await client.ExecuteAsync(request);

            return response.Content;
        }
    }
}
