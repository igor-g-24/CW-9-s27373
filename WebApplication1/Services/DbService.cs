using WebApplication1.DTOs;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public interface IDbService
    {
        Task<PatientDetailsResponseDto> GetPatientDetailsAsync(int idPatient);
        Task<bool> AddPrescriptionAsync(AddPrescriptionRequestDto request);
    }
}