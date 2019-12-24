using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNet.OData;
using Microsoft.Extensions.Logging;
using SelectImprovement.Models;

namespace SelectImprovement.Controllers
{
    public class CustomersController : ODataController
    {
        private readonly ILogger<CustomersController> _logger;
        private IDataRepository _repository;

        public CustomersController(IDataRepository repository, ILogger<CustomersController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_repository.GetCustomers());
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_repository.GetCustomers().FirstOrDefault(c => c.Id == key));
        }
    }
}
