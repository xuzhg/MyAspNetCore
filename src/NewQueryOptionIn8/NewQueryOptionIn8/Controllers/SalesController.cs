using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using NewQueryOptionIn8.Models;

namespace NewQueryOptionIn8.Controllers
{
    public class SalesController : Controller
    {
        IProductSaleRepository _repository;

        public SalesController(IProductSaleRepository repo)
        {
            _repository = repo;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_repository.Sales);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_repository.Sales.FirstOrDefault(s => s.Id == key));
        }
    }
}
