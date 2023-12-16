using BackendHotels.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendHotels.Services.Interfaces
{
    public interface IProductsService
    {
        Task<PaginatedProductsDto> GetProductsPaginatedAsync(string? filterByNameValue = null, string? filterByCategoryValue = null, string? orderByValue = null, string? orderByDirection = null);
    }
}
