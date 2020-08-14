using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dziennik_Admina.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("ID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_User { get; set; }

        [Column("USERNAME", Order = 1)]
        [Required(ErrorMessage = "Pole Username jest wymagane")]
        public string Username { get; set; }

        [Column("PASWORD", Order = 2)]
        [Required(ErrorMessage ="Pole Password jest wymagane")]
        public string Password { get; set; }
        
        public ICollection<Users_Roles> Users_Roles { get; set; }
    }
}
