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
            Assert.NotNull(results);
            Assert.True(results.Count == 1);
        }

        [Fact]
        public async void GetUniversity_ByUniversity_ReturnFalse()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("pests", "");
            Assert.NotNull(results);
            Assert.False(results.Count == 1);
        }
        [Fact]
        public async void GetUniversity_ByCountry_ReturnTrue()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("", "Nepal");
            Assert.NotNull(results);
            Assert.True(results.Count > 0);
        }

        [Fact]
        public async void GetUniversity_ByCountry_ReturnFalse()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("", "Lepal");
            Assert.NotNull(results);
            Assert.False(results.Count == 1);
        }

        [Fact]
        public async void GetUniversity_ByCountry_UniversityName_ReturnTrue()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("Pokhara", "Nepal");
            Assert.NotNull(results);
            Assert.True(results.Count == 1);
        }
        [Fact]
        public async void GetUniversity_ByWrongCountry_UniversityName_ReturnTrue()
        {
            var universityClient = new UniversityClient();
            var results = await universityClient.GetUniversity("Pokhara", "India");
            Assert.NotNull(results);
            Assert.True(results.Count == 0);
        }
    }
}
