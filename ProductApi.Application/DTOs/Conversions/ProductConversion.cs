using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs.Conversions
{
    public static class ProductConversion
    {
        public static ProductDto ToDto(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            return new ProductDto(product.Id, product.Name, product.Quantity, product.Price);
        }
        public static Product ToEntity(this ProductDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Quantity = dto.Quantity,
                Price = dto.Price
            };
        }

        public static (ProductDto?, IEnumerable<ProductDto>?) FromEntity(Product product, IEnumerable<Product>? products )
        {
            // Return Singe ProductDto if products is not null
            if (product is not null || products is null)
            {
                var singleProduct = new ProductDto(
                    product!.Id,
                    product.Name!,
                    product.Quantity,
                    product.Price
                    );

                return (singleProduct, null);
            }
            // Return IEnumerable<ProductDto> if products is not null
            if (products is not null || product is null)
            {
                var _products = products!.Select(pDto =>
                    new ProductDto(
                        pDto.Id,
                        pDto.Name!,
                        pDto.Quantity,
                        pDto.Price
                    )).ToList();
                return (null, _products);
            }

            return (null, null);
        }
    }
}
