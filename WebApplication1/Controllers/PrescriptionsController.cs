using WebApplication1.DTOs;
using WebApplication1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public PrescriptionsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var success = await _dbService.AddPrescriptionAsync(request);
                if (!success)
                {
                    
                    return BadRequest("Failed to add prescription. Please check input data and business rules.");
                }
                return Ok("Prescription added successfully."); 
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (Exception) 
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}