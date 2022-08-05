using GlobalUniversityApp.Client;
using System;
using Xunit;

namespace GlobalUniversityApp.Tests.Client
{
    public class UniversitySearch
    {
        
        [Fact]        
        public async void GetUniversity_ByUniversity_ReturnTrue()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("pokhara","");
            Assert.True(results.Count == 1, "Returning valid result");
        }

        [Fact]
        public async void GetUniversity_ByUniversity_ReturnFalse()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("pests", "");
            Assert.False(results.Count == 1, "having invalid university ");
        }
        [Fact]
        public async void GetUniversity_ByCountry_ReturnTrue()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("", "Nepal");
            Assert.True(results.Count > 0, "University list in Nepal");
        }

        [Fact]
        public async void GetUniversity_ByCountry_ReturnFalse()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("", "Lepal");
            Assert.False(results.Count == 1, "University list by invalid country");
        }

        [Fact]
        public async void GetUniversity_ByCountry_UniversityName_ReturnTrue()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("Pokhara", "Nepal");
            Assert.True(results.Count == 1, "Returning University by name by Country");
        }
        [Fact]
        public async void GetUniversity_ByWrongCountry_UniversityName_ReturnTrue()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("Pokhara", "India");
            Assert.True(results.Count == 0, "Returning University by name by wrong Country");
        }
    }
}
