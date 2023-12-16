using BackendHotels.Contracts.Dtos;
using BackendHotels.Services.Interfaces;
using Newtonsoft.Json;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BackendHotels.Services
{
    public class ProductsService : IProductsService
    {
        private readonly string _prefix = "products";
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;
        public ProductsService(string apiKey, HttpClient httpClient) 
        {
           _apiKey = apiKey;
           _httpClient = httpClient;
        }

        public async Task<PaginatedProductsDto> GetProductsPaginatedAsync(string? filterByNameValue = null, string? filterByCategoryValue = null, string? orderByValue = null, string? orderByDirection = null)
        {
            var url = BuildGetProductsUrl(filterByNameValue, filterByCategoryValue, orderByValue, orderByDirection);

            var responseString = await _httpClient.GetStringAsync(url);

            var products = JsonConvert.DeserializeObject<PaginatedProductsDto>(responseString);

            return products;
        }

        public async Task GetProductDetailsAsync()
        {

        }

        private string BuildGetProductsUrl(string? filterByNameValue = null, string? filterByCategoryValue = null, string? orderByValue = null, string? orderByDirection = null)
        {
            List<string> listOfParameters = new List<string>();
            if(!string.IsNullOrEmpty(filterByNameValue))
            {
                listOfParameters.Add($"name={filterByNameValue}");
            };

            if (!string.IsNullOrEmpty(filterByCategoryValue))
            {
                listOfParameters.Add($"categoryPath.name={filterByCategoryValue}");
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

            var url = _prefix + $"{filterByConstructor}?format=json&pageSize=3&page=10{orderConstructor}&apiKey={_apiKey}";

            return url;
        }
    }
}
