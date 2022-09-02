using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OmitNullPropertySample.Models;

namespace OmitNullPropertySample.Controllers
{
    [ApiController]
    [Route("odata")]
    public class SchoolStudentController : ODataController
    {
        private readonly ISchoolStudentRepository _repo;

        public SchoolStudentController(ISchoolStudentRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("Schools")]
        [HttpGet("Schools/{key}")]
        [EnableQuery]
        public IActionResult GetSchool(int? key)
        {
            if (key != null)
            {
                return Ok(_repo.Schools.FirstOrDefault(s => s.ID == key.Value));
            }
            else
            {
                return Ok(_repo.Schools);
            }
        }

        [HttpGet("Students")]
        [HttpGet("Students/{key}")]
        [EnableQuery]
        public IActionResult GetStudent(int? key)
        {
            if (key != null)
            {
                return Ok(_repo.Students.FirstOrDefault(s => s.ID == key.Value));
            }
            else
            {
                return Ok(_repo.Students);
            }
        }
    }
}