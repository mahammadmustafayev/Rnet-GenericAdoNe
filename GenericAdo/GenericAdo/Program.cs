

using DbHelper.Services;
using GenericAdo.Models;
using GenericAdo.Services;

namespace GenericAdo;

internal class Program
{
    static void Main(string[] args)
    {
        
        ConfigurationService.Configure();
        Category category = new();
        var categoriesV1=category.GetAll();
        //var categoriesV2=category.GetAll("CategoryName","Description");

        Product p = new();
        var produtcsV1 = p.GetAll();
        //var produtcsV2 = p.GetAll("ProductName","CategoryId", "UnitPrice", "UnitsInStock");


        
    }
}