using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using NewQueryOptionIn8.Models;

namespace NewQueryOptionIn8.Controllers
{
    public class ProductsController : Controller
    {
        IProductSaleRepository _repository;

        public ProductsController(IProductSaleRepository repo)
        {
            _repository = repo;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_repository.Products);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_repository.Products.FirstOrDefault(p => p.Id == key));
        }
    }
}
