using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Prescription
    {
        [Key]
        public int IdPrescription { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int IdPatient { get; set; }

        [ForeignKey(nameof(IdPatient))]
        public virtual Patient Patient { get; set; }

        [Required]
        public int IdDoctor { get; set; }

        [ForeignKey(nameof(IdDoctor))]
        public virtual Doctor Doctor { get; set; }

        public virtual ICollection<PrescriptionMedicament> PrescriptionMedicaments { get; set; } = new HashSet<PrescriptionMedicament>();
    }
}