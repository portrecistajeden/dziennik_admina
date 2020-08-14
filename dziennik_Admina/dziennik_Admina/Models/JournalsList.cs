using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dziennik_Admina.Models
{
    [Table("Journals")]
    public class JournalsList
    {
        [Key]
        [Column("ID_JOURNAL", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID_Journal { get; set; }

        [Column("NAME_JOURNAL", Order = 1)]
        [Required(ErrorMessage = "Pole Nazwa jest wymagane")]
        public string Name_journal { get; set; }
    }
}
