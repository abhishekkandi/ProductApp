using System;
using System.Collections.Generic;

namespace ProductApp.Application.DTOS
{
    public class PaginatedProductsDto
    {
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}