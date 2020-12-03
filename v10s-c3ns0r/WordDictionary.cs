using Newtonsoft.Json;

namespace v10s_c3ns0r
{
    public class WordDictionary : IWordDictionary
    {
        [JsonProperty("whitelist")]
        public string[] WhiteList { get; set; }
        [JsonProperty("blacklist")]
        public string[] BlackList { get; set; }
        [JsonProperty("replacements")]
        public string[] Replacements { get; set; }
    }
}