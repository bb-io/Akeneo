using Apps.Akeneo.Models.Entities;

namespace Apps.Akeneo.Models.Response.Product;

public record ListProductResponse(List<ProductEntity> Products);