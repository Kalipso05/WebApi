namespace ModelDataBase.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Hospitalization")]
    public partial class Hospitalization
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public int IDPatient { get; set; }

        public int IDCodeHospitalization { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateOfHospitalization { get; set; }

        public TimeSpan TimeOfHospitalization { get; set; }

        public bool StatusHospitalization { get; set; }

        [Required]
        [StringLength(50)]
        public string ReasonRejection { get; set; }

        public virtual CodeHospitalization CodeHospitalization { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
