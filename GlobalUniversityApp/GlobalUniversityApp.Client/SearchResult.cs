using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlobalUniversityApp.Client
{
    public class SearchResult
    {
        [JsonProperty("web_pages")]
        public List<string> WebPages { get; set; }

        [JsonProperty("state-province")]
        public object StateProvince { get; set; }

        [JsonProperty("alpha_two_code")]
        public string AlphaTwoCode { get; set; }

        [JsonProperty("domains")]
        public List<string> Domains { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
