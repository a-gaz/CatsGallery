using Newtonsoft.Json;

namespace CatsGallery.Shared.Configuration;

public class Settings
{
    public const string DefaultConfigFile = "Configuration/config.json";
    
    [JsonProperty("database")]
    public MinioModel Database { get; set; }
    
    [JsonProperty("minio")]
    public DatabaseModel Minio { get; set; }
    
    [JsonProperty("util")]
    public UtilModel Util { get; set; }
    
    public class MinioModel
    {
        [JsonProperty("endpoint")]
        public string EndPoint { get; set; }
        [JsonProperty("access_key")]
        public string AccessKey { get; set; }
        [JsonProperty("secret_key")]
        public string SecretKey { get; set; }
    }
    
    public class DatabaseModel
    {
        [JsonProperty("connection_string")]
        public string ConnectionString { get; set; }
    }

    public class UtilModel
    {
        [JsonProperty("cats_num_in_page")]
        public string CatsNumInPage { get; set; }
    }
}