using System.Collections.Generic;
using System.IO;
using Atlas.Core.Analysis;
using Atlas.Core.Internal;
using Newtonsoft.Json;

namespace Atlas.Core
{
    public class Project
    {
        [JsonProperty(Required = Required.Always)] public string BaseDirectory { get; set; }
        [JsonProperty(Required = Required.Always)] public string OutputDirectory { get; set; }
        [JsonProperty(Required = Required.Always)] public IEnumerable<string> Assemblies { get; set; }
        
        public IEnumerable<string> ExtraProbePaths { get; } = new List<string>();
        public IEnumerable<Option> Settings { get; } = new List<Option>();
        public IList<string> Plugins { get; } = new List<string>();
        
        public static Project FromFile(string path) => JsonConvert.DeserializeObject<Project>(File.ReadAllText(path));
        public void Save(string path) => File.WriteAllText(path, JsonConvert.SerializeObject(this, Formatting.Indented));

        public void Run(ILogger logger)
        {
            Engine.Initialise(this, logger);
            Engine.Run();
        }
    }
}