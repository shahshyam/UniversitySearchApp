using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GlobalUniversityApp.Client
{
    public class UniversityClient : IUniversityClient
    {
        private HttpClient _httpClient;
        public UniversityClient()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://hipolabs.com");
        }
        public async Task<List<SearchResult>> GetUniversity(string universityName, string country)
        {
            var results = new List<SearchResult>();
            string url = "search?";
            if (!string.IsNullOrEmpty(universityName) && !string.IsNullOrEmpty(country))
                url += $"name={universityName}&country={country}";
            else if (!string.IsNullOrEmpty(country))
                url += $"country={country}";
            else if(!string.IsNullOrEmpty(universityName))
                url += $"name={universityName}";
            var response = await _httpClient.GetAsync(url);
            string content=await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                results = JsonConvert.DeserializeObject<List<SearchResult>>(content);
            }
            return results;
        }
    }
}
