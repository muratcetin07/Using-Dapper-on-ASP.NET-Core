using AutoFixture;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTest
{
    public class ProductControllerTests
    {
        private readonly HttpClient _client;

        public ProductControllerTests()
        {
            var testServer = new TestServer(new WebHostBuilder()
             .UseStartup<TestStartup>());

            _client = testServer.CreateClient();
        }

        [Fact]
        public async Task Get_Should_Return_OK_Product_By_Id()
        {
            var expectedResult = string.Empty;

            int id = 10;
            var response = await _client.GetAsync($"/api/product/GetProductById/{id}");
            var actualStatusCode = response.StatusCode;
            var actualResult = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedResult, actualResult);

        }


        [Theory]
        [InlineData("/api/product/saveProduct", "/api/product/getProductById/{id}")]
        public async Task Get_Should_Return_OK_With_Inserted_Product_When_Insert_Success(string postUrl, string getUrl)
        {
            var expectedResult = string.Empty;
            var expectedStatusCode = HttpStatusCode.OK;

            //auto generate input data
            Fixture fixture = new Fixture();
            var inputData = fixture.Create<Product>();

            var content = new StringContent(JsonConvert.SerializeObject(inputData), Encoding.UTF8, "application/json");

            // Act-1
            var response = await _client.PostAsync(postUrl, content);
            var actualStatusCode = response.StatusCode;
            var actualResult = await response.Content.ReadAsStringAsync();

            // Assert-1
            Assert.Equal(expectedResult, actualResult);
            Assert.Equal(expectedStatusCode, actualStatusCode);

            // Act-2
             getUrl = getUrl.Replace("{id}", inputData.Id.ToString());
            var responseGet = await _client.GetAsync(getUrl);

            var actualGetResult = await responseGet.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Product>(actualGetResult);

            var insertedProductExist = result.ProductName == inputData.ProductName ? true : false;

            // Assert-2
            Assert.NotNull(result);
            Assert.True(insertedProductExist);

        }
    }
}
