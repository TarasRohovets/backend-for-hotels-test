using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendHotels.Contracts.Dtos
{
    public class ProductDto
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public string SalePrice { get; set; }  
        public string Image { get; set; }
        public string ThumbnailImage { get; set; }
    }
}
