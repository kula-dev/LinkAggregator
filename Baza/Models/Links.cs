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

        [Url(ErrorMessage = "Proszę podać prawidłowy link")]
        public string Link { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        public virtual Users Users { get; set; }

        public virtual ICollection<Likes> Likes { get; set; }
    }
}
