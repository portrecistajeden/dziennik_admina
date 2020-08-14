using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dziennik_Admina.TabItems
{
    public class Entry
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Username { get; set; }
        public string Text { get; set; }
        public bool isButtonEnabled { get; set; }
    }
}
