using ASP.NET_Demo.Controllers;
using ASP.NET_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Demo_Test;
public class ProductControllerTests
{

    [Fact]
    public void Index_ReturnsViewResult_WithListOfProducts()
    {
        // Arrange
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(repo => repo.GetAllProducts())
            .Returns(GetTestProducts());
        var controller = new ProductController(mockRepo.Object);

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result); // The result is checked to ensure it is a ViewResult.
        var model = Assert.IsAssignableFrom<IEnumerable<Product>>(
            viewResult.ViewData.Model); // The model of the ViewResult is checked to ensure it is a list of products.
        Assert.Equal(2, model.Count()); // The count of the products in the model is verified to be 2.
    }

    private List<Product> GetTestProducts()
    {
        return new List<Product>
        {
            new Product { ProductID = 1, Name = "Product1", Price = 10 },
            new Product { ProductID = 2, Name = "Product2", Price = 20 }
        };
    }

    [Fact]
    public void ViewProduct_ReturnsViewResult_WithProduct()
    {
        //Arrange
        var mockRepo = new Mock<IProductRepository>();
        var testProduct = new Product { ProductID = 1, Name = "Product1", Price = 10 };
        mockRepo.Setup(repo => repo.GetProduct(1))
            .Returns(testProduct);
        var controller = new ProductController(mockRepo.Object);

        //Act
        var result = controller.ViewProduct(1);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
        Assert.Equal(testProduct, model);

    }


    [Fact]
    public void ViewProduct_ReturnsNotFoundResult_WhenProductNotFound()
    {
        //Arrange
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(repo => repo.GetProduct(1)).Returns((Product)null);
        var controller = new ProductController(mockRepo.Object);

        //Act
        var result = controller.ViewProduct(1);

        //Assert
         Assert.IsType<NotFoundResult>(result);
        //Assert.Equal("ProductNotFound", result);
    }

    [Fact]
    public void UpdateProduct_ReturnsViewResult_WithProduct()
    {
        //Arrange
        var mockRepo = new Mock<IProductRepository>();
        var testProduct = new Product { ProductID = 1, Price = 10 };
        mockRepo.Setup(repo => repo.GetProduct(1))
            .Returns(testProduct);
        var controller = new ProductController(mockRepo.Object);

        //Act
        var result = controller.UpdateProduct(1);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
        Assert.Equal(testProduct, model);
    }

    [Fact]
    public void UpdateProduct_ReturnsProductNotFoundView_WhenProductNotFound()
    {
        //Arrange
        var mockRepo = new Mock<IProductRepository>();
        mockRepo.Setup(repo => repo.GetProduct(1)).Returns((Product)null);
        var controller = new ProductController(mockRepo.Object);

        //Act
        var result = controller.UpdateProduct(1);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("ProductNotFound", viewResult.ViewName);
    }

    [Fact]
    public void DeleteProduct_RedirectsToIndex()
    {
        //Arrange
        var mockRepo = new Mock<IProductRepository>();
        var testProduct = new Product { ProductID = 1, Price = 10 };
        mockRepo.Setup(repo => repo.DeleteProduct(1)).Returns(testProduct);

        //Act

        //Assert
    }
}
