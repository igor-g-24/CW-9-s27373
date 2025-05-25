using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class PrescriptionMedicament
    {
        [Key]
        [Column(Order = 0)]
        public int IdMedicament { get; set; }

        [ForeignKey(nameof(IdMedicament))]
        public virtual Medicament Medicament { get; set; }

        [Key]
        [Column(Order = 1)]
        public int IdPrescription { get; set; }

        [ForeignKey(nameof(IdPrescription))]
        public virtual Prescription Prescription { get; set; }

        public int? Dose { get; set; } 

        [MaxLength(100)]
        public string Details { get; set; }
    }
}