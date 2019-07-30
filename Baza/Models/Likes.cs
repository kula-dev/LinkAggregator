using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Baza.Models
{
    public class Likes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LikeId { get; set; }

        [ForeignKey("Users")]
        public int UserID { get; set; }

        [ForeignKey("Links")]
        public int LinkID { get; set; }

        public virtual Links Links { get; set; }

        public virtual Users Users { get; set; }
    }
}
