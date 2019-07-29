using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Baza.Models
{
    public class Links
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LinkId { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public virtual Users Users { get; set; }
    }
}
