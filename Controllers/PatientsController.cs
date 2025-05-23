using APBD_EF_CodeFirst_Example.DTOs;
using APBD_EF_CodeFirst_Example.Exceptions;
using APBD_EF_CodeFirst_Example.Services;
using Microsoft.AspNetCore.Mvc;

namespace APBD_EF_CodeFirst_Example.Controllers;

[ApiController]
[Route("api/patients")]
public class PatientsController(IDbService service) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatientDetails([FromRoute] int id)
    {
        try
        {
            return Ok(await service.GetPatientDetailsAsync(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    
}