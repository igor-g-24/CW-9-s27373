using System;
using System.Collections.Generic;

namespace WebApplication1.DTOs
{
    public class PatientDetailsResponseDto
    {
        public int IdPatient { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public List<PrescriptionDetailDto> Prescriptions { get; set; }
    }

    public class PrescriptionDetailDto
    {
        public int IdPrescription { get; set; }
        public DateTime Date { get; set; }
        public DateTime DueDate { get; set; }
        public DoctorDto Doctor { get; set; }
        public List<MedicamentDetailDto> Medicaments { get; set; }
    }

    public class MedicamentDetailDto
    {
        public int IdMedicament { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public string Type { get; set; }
        public int? Dose { get; set; } 
        public string Details { get; set; } 
    }

    public class DoctorDto
    {
        public int IdDoctor { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}