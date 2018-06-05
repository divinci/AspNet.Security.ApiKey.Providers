using System.Net;
using System.Threading.Tasks;
using AspNet.Security.ApiKey.Providers.Web;
using FluentAssertions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNet.Security.ApiKey.Providers.Tests
{
    [TestClass]
    public class AuthenticationTests
    {
        private TestServer api;

        [TestInitialize]
        public void Initialize()
        {
            this.api = new TestServer(WebHost
                .CreateDefaultBuilder()
                .UseStartup<Startup>());
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.api.Dispose();
        }

        [TestMethod]
        public async Task Access_Anonymous_Resource_Using_Anonymous_Authentication_Should_Yield_200()
        {
            var response = await this.api.CreateClient().GetAsync("/api/anonymous/values");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Access_Anonymous_Resource_Using_Valid_ApiKey_Authentication_Should_Yield_200()
        {
            var response = await this.api.CreateRequest("/api/anonymous/values").AddHeader("Authorization", "ApiKey 123").GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Access_Anonymous_Resource_Using_Invalid_ApiKey_Authentication_Should_Yield_401()
        {
            var response = await this.api.CreateRequest("/api/anonymous/values").AddHeader("Authorization", "ApiKey 456").GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Access_Protected_Resource_Using_Anonymous_Authentication_Should_Yield_401()
        {
            var response = await this.api.CreateClient().GetAsync("/api/authenticated/values");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public async Task Access_Protected_Resource_Using_Valid_ApiKey_Authentication_Should_Yield_200()
        {
            var response = await this.api.CreateRequest("/api/authenticated/values").AddHeader("Authorization", "ApiKey 123").GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task Access_Protected_Resource_Using_Invalid_ApiKey_Authentication_Should_Yield_401()
        {
            var response = await this.api.CreateRequest("/api/authenticated/values").AddHeader("Authorization", "ApiKey 456").GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public async Task Access_Protected_Resource_Using_Custom_Failure_Criteria_Should_Yield_Custom_Status_Code()
        {
            var response = await this.api.CreateRequest("/api/authenticated/values").AddHeader("Authorization", "ApiKey 789").GetAsync();

            response.StatusCode.Should().Be(HttpStatusCode.UpgradeRequired);
        }
    }
}
