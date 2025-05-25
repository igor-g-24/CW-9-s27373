using WebApplication1.DTOs;
using WebApplication1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IDbService _dbService;

        public PatientsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("{idPatient}")]
        public async Task<IActionResult> GetPatientDetails(int idPatient)
        {
            var patientDetails = await _dbService.GetPatientDetailsAsync(idPatient);
            if (patientDetails == null)
            {
                return NotFound($"Patient with ID {idPatient} not found.");
            }
            return Ok(patientDetails);
        }
    }
}