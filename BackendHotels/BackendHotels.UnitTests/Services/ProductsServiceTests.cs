using BackendHotels.Contracts.Dtos;
using BackendHotels.Services;
using BackendHotels.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Net;

namespace BackendHotels.UnitTests.Services
{
    [TestFixture]
    public class ProductsServiceTests
    {
        private IProductsService _productsService;
        private Mock<HttpMessageHandler> _httpClientMock;
        private Mock<ILogger<ProductsService>> _logger;

        [SetUp]
        public void Setup() 
        {
            _httpClientMock = new Mock<HttpMessageHandler>();
            _logger = new Mock<ILogger<ProductsService>>();

            var httpClient = new HttpClient(_httpClientMock.Object);
            httpClient.BaseAddress = new Uri("https://test.com");

            _productsService = new ProductsService(It.IsAny<string>(),
                httpClient,
                _logger.Object
            );
        }

        [Test]
        public async Task GetProductsPaginatedAsync_WithValidParameters_ReturnsPaginatedGames()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Name = "test", Sku = "test", SalePrice = "test", Image = "test", ThumbnailImage = "test" },
                new ProductDto { Name = "test", Sku = "test", SalePrice = "test", Image = "test", ThumbnailImage = "test" }
            };

            var paginatedDto = new PaginatedProductsDto()
            {
                Products = products,
                TotalPages = 2,
                CurrentPage = 1
            };

            var responseString = JsonConvert.SerializeObject(paginatedDto);

            _httpClientMock.Protected()
                  .Setup<Task<HttpResponseMessage>>("SendAsync", true,
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.OK,
                      Content = new StringContent(responseString)
                  }).Verifiable();

            // Act
            var result = await _productsService.GetProductsPaginatedAsync();

            // Assert
            ClassicAssert.AreEqual(paginatedDto.Products.First().Name, result.Products.First().Name);
        }

        [Test]
        public async Task GetProductsPaginatedAsync_HttpClientThrowsException()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Name = "test", Sku = "test", SalePrice = "test", Image = "test", ThumbnailImage = "test" },
                new ProductDto { Name = "test", Sku = "test", SalePrice = "test", Image = "test", ThumbnailImage = "test" }
            };

            var paginatedDto = new PaginatedProductsDto()
            {
                Products = products,
                TotalPages = 2,
                CurrentPage = 1
            };

            var responseString = JsonConvert.SerializeObject(paginatedDto);

            _httpClientMock.Protected()
                  .Setup<Task<HttpResponseMessage>>("SendAsync", true,
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
                  .ReturnsAsync(new HttpResponseMessage
                  {
                      StatusCode = HttpStatusCode.BadRequest
                  }).Verifiable();

            // Act
            Assert.ThrowsAsync<HttpRequestException>(async () => await _productsService.GetProductsPaginatedAsync());
        }
    }
}
