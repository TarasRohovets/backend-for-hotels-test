using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendHotels.Contracts.Dtos
{
    public class PaginatedProductsDto
    {
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
