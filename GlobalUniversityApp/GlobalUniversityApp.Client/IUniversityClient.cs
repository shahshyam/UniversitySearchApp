using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GlobalUniversityApp.Client
{
    public interface IUniversityClient
    {
        Task<List<SearchResult>> GetUniversity(string universityName, string country);
    }
}
