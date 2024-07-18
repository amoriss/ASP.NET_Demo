using Dapper;
using System.Data;

namespace ASP.NET_Demo.Models;

public class ProductRepository : IProductRepository
{

    private readonly IDbService _dbService;

    public ProductRepository(IDbService dbService)
    {
        _dbService = dbService;
    }

 

    public IEnumerable<Product> GetAllProducts()
    {
        return _dbService.Query<Product>("SELECT * FROM PRODUCTS;");
    }

    public Product GetProduct(int id)
    {
        return _dbService.QuerySingle<Product>("SELECT * FROM PRODUCTS WHERE PRODUCTID = @id", new { id = id });
    }
    public void UpdateProduct(Product product)
    {
       var query = _dbService.Execute("UPDATE products SET Name = @name, Price = @price WHERE ProductID = @id",
         new { name = product.Name, price = product.Price, id = product.ProductID });
    }
    public void InsertProduct(Product productToInsert)
    {
        _dbService.Execute("INSERT INTO products (NAME, PRICE, CATEGORYID) VALUES (@name, @price, @categoryID);",
            new { name = productToInsert.Name, price = productToInsert.Price, categoryID = productToInsert.CategoryID });
    }
    public IEnumerable<Category> GetCategories()
    {
        return _dbService.Query<Category>("SELECT * FROM categories;");
    }
    public Product AssignCategory()
    {
        var categoryList = GetCategories();
        var product = new Product();
        product.Categories = categoryList;
        return product;
    }
    public void DeleteProduct(Product product)
    {
        _dbService.Execute("DELETE FROM REVIEWS WHERE ProductID = @id;", new { id = product.ProductID });
        _dbService.Execute("DELETE FROM Sales WHERE ProductID = @id;", new { id = product.ProductID });
        _dbService.Execute("DELETE FROM Products WHERE ProductID = @id;", new { id = product.ProductID });
    }

}
