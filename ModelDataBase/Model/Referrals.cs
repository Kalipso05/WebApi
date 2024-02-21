namespace ModelDataBase.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Referrals
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int IDMedicalCard { get; set; }

        public int IDDoctor { get; set; }

        [Column(TypeName = "date")]
        public DateTime CreationDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Anamnesis { get; set; }

        [Required]
        [StringLength(50)]
        public string Symptoms { get; set; }

        [Required]
        [StringLength(50)]
        public string Diagnosis { get; set; }

        [Required]
        public string Recomendation { get; set; }

        public virtual Doctor Doctor { get; set; }

        public virtual MedicalCard MedicalCard { get; set; }
    }
}
