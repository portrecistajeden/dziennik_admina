using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dziennik_Admina.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [Column("ID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Role { get; set; }

        [Column("NAME", Order = 1)]
        [Required(ErrorMessage ="Pole Nazwa jest wymagane")]
        public string Name { get; set; }

        public ICollection<Users_Roles> Users_Roles { get; set; }
    }
}
