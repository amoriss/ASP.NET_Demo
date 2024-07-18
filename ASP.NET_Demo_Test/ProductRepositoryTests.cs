using ASP.NET_Demo.Models;
using Dapper;
using Moq;
using System.Data;

namespace ASP.NET_Demo_Test;

public class ProductRepositoryTests
{

    [Fact]
    public void GetAllProducts_ReturnsAllProducts()
    {
        // Arrange
        var mockDbService = new Mock<IDbService>();
        var mockProducts = new List<Product>
        {
            new Product { ProductID = 1, Name = "Product1", Price = 10 },
            new Product { ProductID = 2, Name = "Product2", Price = 20 }
        };

        mockDbService
           .Setup(db => db.Query<Product>("SELECT * FROM PRODUCTS;", null))
           .Returns(mockProducts);

        var productRepository = new ProductRepository(mockDbService.Object);

        // Act
        var result = productRepository.GetAllProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Product1", result.ElementAt(0).Name);
    }

    [Fact]
    public void GetProduct_ReturnsProduct()
    {
        // Arrange
        var mockDbService = new Mock<IDbService>();
        var mockProduct = new Product { ProductID = 1, Name = "Product1", Price = 10 };

        mockDbService
            .Setup(db => db.QuerySingle<Product>("SELECT * FROM PRODUCTS WHERE Id = @Id", It.IsAny<object>()))
            .Returns(mockProduct);

        var productRepository = new ProductRepository(mockDbService.Object);

        // Act
        var result = productRepository.GetProduct(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.ProductID);
        Assert.Equal("Product1", result.Name);
    }


    [Fact]
    public void UpdateProduct_ExecutesUpdate()
    {
        // Arrange
        var mockDbService = new Mock<IDbService>();
        var product = new Product { ProductID = 1, Name = "UpdatedProduct", Price = 15 };

        mockDbService
         .Setup(db => db.Execute(
             It.Is<string>(s => s == "UPDATE products SET Name = @name, Price = @price WHERE Id = @id"),
             It.IsAny<object>()))
         .Returns(1);

        var productRepository = new ProductRepository(mockDbService.Object);

        // Act
        productRepository.UpdateProduct(product);

        // Assert
        mockDbService.Verify(db => db.Execute(
             "UPDATE products SET Name = @name, Price = @price WHERE Id = @id",
             It.Is<object>(param =>
                 param.GetType().GetProperty("name").GetValue(param).Equals(product.Name) &&
                 param.GetType().GetProperty("price").GetValue(param).Equals(product.Price) &&
                 param.GetType().GetProperty("id").GetValue(param).Equals(product.ProductID)
             )),
             Times.Once);
    }
}