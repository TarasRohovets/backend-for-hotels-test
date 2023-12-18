using BackendHotels.Contracts.Dtos;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BackendHotels.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSettingsController : ControllerBase
    {
        private IMemoryCache _cache;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private string _orderByKey = "orderByKey";
        private string _sortKey = "sortKey";
        private string _inputValueKey = "inputValueKey";
        private string _inputTypeKey = "inputTypeKey";
        private string _currentPageKey = "currentPageKey";
        public UserSettingsController(IMemoryCache cache) 
        {
            _cache= cache;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetUserSettingsAsync()
        {
            var userSettings = new UserSettingsDto();

            if (_cache.TryGetValue(_orderByKey, out string? orderByValue))
            {
                userSettings.OrderBy = orderByValue;
            }

            if (_cache.TryGetValue(_sortKey, out string? sortValue))
            {
                userSettings.Sort = sortValue;
            }
            if (_cache.TryGetValue(_inputValueKey, out string? inputValue))
            {
                userSettings.InputValue = inputValue;
            }
            if (_cache.TryGetValue(_inputTypeKey, out string? inputTypeValue))
            {
                userSettings.InputType = inputTypeValue;
            }
            if (_cache.TryGetValue(_currentPageKey, out string? currentPageValue))
            {
                if (!string.IsNullOrEmpty(currentPageValue))
                {
                    userSettings.CurrentPage = int.Parse(currentPageValue);
                }
                
            }

            return Ok(userSettings);
        }

        [HttpPatch("{key}/{value}")]
        public async Task<IActionResult> UpdateUserSettingsAsync(string key, string value)
        {
            try
            {
                await semaphore.WaitAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   .SetSlidingExpiration(TimeSpan.FromDays(3))
                   .SetAbsoluteExpiration(TimeSpan.FromDays(7))
                   .SetPriority(CacheItemPriority.Normal);

                if(value == "undefined")
                {
                    _cache.Remove(key);
                } else
                {
                    _cache.Set(key, value, cacheEntryOptions);
                }
                

                if(key == _inputValueKey)
                {
                    _cache.Remove(_orderByKey);
                    _cache.Remove(_sortKey);
                    _cache.Remove(_currentPageKey);
                }

            } finally
            {
                semaphore.Release();
            }

            return Ok();
        }
    }
}
