using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dziennik_Admina.Models
{
    public class Journal3
    {
        [Key]
        [Column("ID", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Column("DATE", Order = 1)]
        public DateTime data { get; set; }
        [Column("USERNAME", Order = 2)]
        public string username { get; set; }
        [Column("ENTRY", Order = 3)]
        public string entry { get; set; }
    }
}
