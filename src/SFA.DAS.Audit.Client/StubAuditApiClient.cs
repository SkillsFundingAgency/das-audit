using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.Audit.Types;

namespace SFA.DAS.Audit.Client
{
    public class StubAuditApiClient : IAuditApiClient
    {
        private readonly string _outputDirectory;

        public StubAuditApiClient()
        {
            var appDataFolder = !string.IsNullOrEmpty((string)AppDomain.CurrentDomain.GetData("DataDirectory"))
                ? (string)AppDomain.CurrentDomain.GetData("DataDirectory")
                : Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "App_Data");

            appDataFolder = Path.Combine(appDataFolder, "audit");

            _outputDirectory = appDataFolder;
        }

        public StubAuditApiClient(string outputDirectory)
        {
            _outputDirectory = outputDirectory;
        }

        public async Task Audit(AuditMessage message)
        {
            if (!Directory.Exists(_outputDirectory))
            {
                Directory.CreateDirectory(_outputDirectory);
            }

            var filePrefix = DateTime.Now.ToString("yyyy-MM-dd-HHmmss");
            var fileCounter = 1;
            var path = Path.Combine(_outputDirectory, $"{filePrefix}-{fileCounter:000}.json");
            while (File.Exists(path))
            {
                fileCounter++;
                path = Path.Combine(_outputDirectory, $"{filePrefix}-{fileCounter:000}.json");
            }

            using (var stream = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
            using (var writer = new StreamWriter(stream))
            {
                await writer.WriteAsync(JsonConvert.SerializeObject(message, Formatting.Indented));
                await writer.FlushAsync();
                writer.Close();
            }
        }
    }
}
