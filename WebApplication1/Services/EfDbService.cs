using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace WebApplication1.Services
{
    public class EfDbService : IDbService
    {
        private readonly AppDbContext _context;

        public EfDbService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddPrescriptionAsync(AddPrescriptionRequestDto request)
        {
            if (request.DueDate < request.Date)
            {
                return false; 
            }

            if (request.Medicaments.Count > 10)
            {
                return false; 
            }

            Patient patient;
            if (request.Patient.IdPatient.HasValue)
            {
                patient = await _context.Patients.FindAsync(request.Patient.IdPatient.Value);
                if (patient == null) 
                {
                    patient = new Patient
                    {
                        FirstName = request.Patient.FirstName,
                        LastName = request.Patient.LastName,
                        Birthdate = request.Patient.Birthdate
                    };
                    await _context.Patients.AddAsync(patient);
                    
                }
            }
            else 
            {
                patient = new Patient
                {
                    FirstName = request.Patient.FirstName,
                    LastName = request.Patient.LastName,
                    Birthdate = request.Patient.Birthdate
                };
                await _context.Patients.AddAsync(patient);
            }
            
            var doctor = await _context.Doctors.FindAsync(request.IdDoctor);
            if (doctor == null)
            {
                return false; 
            }

            foreach (var medDto in request.Medicaments)
            {
                var medicamentExists = await _context.Medicaments.AnyAsync(m => m.IdMedicament == medDto.IdMedicament);
                if (!medicamentExists)
                {
                    return false; 
                }
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {

                    await _context.SaveChangesAsync(); 

                    var prescription = new Prescription
                    {
                        Date = request.Date,
                        DueDate = request.DueDate,
                        IdPatient = patient.IdPatient, 
                        Patient = patient,
                        IdDoctor = request.IdDoctor,
                        Doctor = doctor
                    };
                    await _context.Prescriptions.AddAsync(prescription);
                    await _context.SaveChangesAsync(); 

                    foreach (var medDto in request.Medicaments)
                    {
                        var prescriptionMedicament = new PrescriptionMedicament
                        {
                            IdMedicament = medDto.IdMedicament,
                            IdPrescription = prescription.IdPrescription,
                            Dose = medDto.Dose,
                            Details = medDto.Details
                        };
                        await _context.PrescriptionMedicaments.AddAsync(prescriptionMedicament);
                    }

                    await _context.SaveChangesAsync(); 
                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception) 
                {
                    await transaction.RollbackAsync();
                    return false; 
                }
            }
        }


        public async Task<PatientDetailsResponseDto> GetPatientDetailsAsync(int idPatient)
        {
            var patient = await _context.Patients
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.Doctor)
                .Include(p => p.Prescriptions)
                    .ThenInclude(pr => pr.PrescriptionMedicaments)
                        .ThenInclude(pm => pm.Medicament)
                .Where(p => p.IdPatient == idPatient)
                .FirstOrDefaultAsync();

            if (patient == null)
            {
                return null;
            }

            var response = new PatientDetailsResponseDto
            {
                IdPatient = patient.IdPatient,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Birthdate = patient.Birthdate,
                Prescriptions = patient.Prescriptions
                    .OrderBy(pr => pr.DueDate) 
                    .Select(pr => new PrescriptionDetailDto
                    {
                        IdPrescription = pr.IdPrescription,
                        Date = pr.Date,
                        DueDate = pr.DueDate,
                        Doctor = new DoctorDto
                        {
                            IdDoctor = pr.Doctor.IdDoctor,
                            FirstName = pr.Doctor.FirstName,
                            LastName = pr.Doctor.LastName
                        },
                        Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentDetailDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Description = pm.Medicament.Description, 
                            Type = pm.Medicament.Type,
                            Dose = pm.Dose,
                            Details = pm.Details 
                        }).ToList()
                    }).ToList()
            };

            return response;
        }
    }
}
