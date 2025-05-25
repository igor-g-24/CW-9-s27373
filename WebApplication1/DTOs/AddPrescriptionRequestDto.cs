using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class AddPrescriptionRequestDto
    {
        [Required]
        public PatientRequestDto Patient { get; set; }

        [Required]
        public List<PrescriptionMedicamentRequestDto> Medicaments { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int IdDoctor { get; set; }
    }

    public class PatientRequestDto
    {
        public int? IdPatient { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthdate { get; set; }
    }

    public class PrescriptionMedicamentRequestDto
    {
        [Required]
        public int IdMedicament { get; set; }
        public int? Dose { get; set; } 

        [Required]
        [MaxLength(100)]
        public string Details { get; set; } 
    }
}