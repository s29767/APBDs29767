using APBD_EF_CodeFirst_Example.Data;
using APBD_EF_CodeFirst_Example.DTOs;
using APBD_EF_CodeFirst_Example.Exceptions;
using APBD_EF_CodeFirst_Example.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD_EF_CodeFirst_Example.Controllers;

[ApiController]
[Route("api/prescriptions")]
public class PrescriptionsController(IDbService service, AppDbContext data) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddPrescription([FromBody] PrescriptionCreateDto prescriptionData)
    {
        try
        {
            var prescription = await service.AddPrescriptionAsync(prescriptionData);
            return CreatedAtAction(nameof(GetPrescriptionDetails), new { id = prescription.IdPrescription }, prescription);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrescriptionDetails(int id)
    {
        try
        {
            var prescription = await data.Prescriptions
                .Include(p => p.Patient)
                .Include(p => p.Doctor)
                .Include(p => p.PrescriptionMedicaments)
                .ThenInclude(pm => pm.Medicament)
                .FirstOrDefaultAsync(p => p.IdPrescription == id);
            
            if (prescription == null)
            {
                return NotFound();
            }
            
            return Ok(prescription);
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}