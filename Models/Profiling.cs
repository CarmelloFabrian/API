using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("TB_M_Profiling")]
    public class Profiling
    {
        [Key]
        public string NIK { get; set; }
        public virtual Account Account { get; set; }
        public virtual Education Education { get; set; }
        [ForeignKey("Education")]
        public int EducationId { get; set; }
    }
}
