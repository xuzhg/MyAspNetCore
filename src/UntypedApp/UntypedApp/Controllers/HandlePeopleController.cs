//-----------------------------------------------------------------------------
// <copyright file="HandlePeopleController.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using UntypedApp.Models;

namespace UntypedApp.Controllers;

[Route("odata")]
public class HandlePeopleController : ODataController
{
    [HttpGet("People")]
    public IActionResult Get()
    {
        return Ok(DataSource.GetPeople());
    }

    [HttpGet("People/{id}")]
    [HttpGet("People({id})")]
    public IActionResult Get(int id)
    {
        var p = DataSource.GetPeople().FirstOrDefault(p => p.Id == id);
        if (p == null)
        {
            return NotFound($"Cannot find a person with Id={id}");
        }

        return Ok(p);
    }

    [HttpPost("People")]
    public IActionResult Post([FromBody]Person person)
    {
        if (!ModelState.IsValid || person == null)
        {
            var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.Exception.Message));
            return BadRequest(message);
        }

        person = DataSource.AddPerson(person);
        return Created(person);
    }

    [HttpGet("People/{id}/Data")]
    [HttpGet("People({id})/Data")]
    public IActionResult GetData(int id)
    {
        var p = DataSource.GetPeople().FirstOrDefault(p => p.Id == id);
        if (p == null)
        {
            return NotFound($"Cannot find a person with Id={id}");
        }

        return Ok(p.Data);
    }

    [HttpGet("People/{id}/Infos")]
    [HttpGet("People({id})/Infos")]
    public IActionResult GetInfos(int id)
    {
        var p = DataSource.GetPeople().FirstOrDefault(p => p.Id == id);
        if (p == null)
        {
            return NotFound($"Cannot find a person with Id={id}");
        }

        return Ok(p.Infos);
    }
}