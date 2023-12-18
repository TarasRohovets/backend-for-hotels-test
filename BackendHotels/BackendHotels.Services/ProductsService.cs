using BackendHotels.Contracts.Dtos;
using BackendHotels.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendHotels.Services
{
    public class ProductsService : IProductsService
    {
        private readonly string _prefix = "products";
        private readonly int _defaultPageSize = 10;
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        ILogger<ProductsService> _logger;
        public ProductsService(string apiKey, HttpClient httpClient, ILogger<ProductsService> logger) 
        {
           _apiKey = apiKey;
           _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<PaginatedProductsDto> GetProductsPaginatedAsync(string? filterByNameValue = null, string? filterByCategoryValue = null, string? orderByValue = null, string? orderByDirection = null, int? page = 1)
        {
            var url = BuildGetProductsUrl(filterByNameValue, filterByCategoryValue, orderByValue, orderByDirection, page);

            string responseString = "";
            try
            {
                responseString = await _httpClient.GetStringAsync(url);

            } catch (Exception ex)
            {
                _logger.LogError("[ProductsService] GetProductsPaginatedAsync: error calling {url}, stackTrace: {ex}", url, ex);
                throw;
            }
           

            var products = JsonConvert.DeserializeObject<PaginatedProductsDto>(responseString);

            return products;
        }

        private string BuildGetProductsUrl(string? filterByNameValue = null, string? filterByCategoryValue = null, string? orderByValue = null, string? orderByDirection = null, int? page = 1)
        {
            List<string> listOfParameters = new List<string>();
            if(!string.IsNullOrEmpty(filterByNameValue))
            {
                var encoded = Uri.EscapeDataString(filterByNameValue);
                listOfParameters.Add($"name={encoded}");
            };

            if (!string.IsNullOrEmpty(filterByCategoryValue))
            {
                var encoded = Uri.EscapeDataString(filterByCategoryValue);
                listOfParameters.Add($"categoryPath.name={encoded}");
            };

            string filterByConstructor = "";
            if (listOfParameters.Count > 0)
            {
                for (int i = 0; i < listOfParameters.Count(); i++)
                {
                    if (i == 0)
                    {
                        filterByConstructor = $"({listOfParameters[i]})";
                    } else
                    {
                        filterByConstructor = filterByConstructor.Replace(")", $"&{listOfParameters[i]})");
                    }
                }
            }
 

            var orderConstructor = (!string.IsNullOrEmpty(orderByValue) && !string.IsNullOrEmpty(orderByDirection)) ?
                $"&sort={orderByValue}.{orderByDirection}" :
                string.Empty;

            var url = _prefix + $"{filterByConstructor}?format=json&pageSize={_defaultPageSize}&page={page}{orderConstructor}&apiKey={_apiKey}";

            return url;
        }
    }
}
