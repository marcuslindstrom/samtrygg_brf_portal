using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using SamtryggBrfPortal.Web;

namespace SamtryggBrfPortal.Tests.Web
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public IntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Get_HomePage_ReturnsSuccessAndCorrectContentType()
        {
            // Act
            var response = await _client.GetAsync("/");

            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            Assert.Equal("text/html; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_AboutPage_ReturnsSuccessAndCorrectContentType()
        {
            // Act
            var response = await _client.GetAsync("/Home/About");

            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            Assert.Equal("text/html; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_ContactPage_ReturnsSuccessAndCorrectContentType()
        {
            // Act
            var response = await _client.GetAsync("/Home/Contact");

            // Assert
            response.EnsureSuccessStatusCode(); // Status code 200-299
            Assert.Equal("text/html; charset=utf-8", 
                response.Content.Headers.ContentType.ToString());
        }

        [Fact]
        public async Task Get_NonExistentPage_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/Home/NonExistentPage");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
