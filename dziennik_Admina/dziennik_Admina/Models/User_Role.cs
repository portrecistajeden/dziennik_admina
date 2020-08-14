using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dziennik_Admina.Models
{
    [Table("Users_Roles")]
    public class Users_Roles
    {
        [Key]
        [Column("ID_USER", Order = 0)]
        public int ID_User { get; set; }

        [Key]
        [Column("ID_ROLE", Order = 1)]
        public int ID_Role { get; set; }

        [ForeignKey("ID_User")]
        public virtual User user { get; set; }

        [ForeignKey("ID_Role")]
        public virtual Role role { get; set; }
    }
}
