using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dziennik_Admina.TabItems
{
    public class Tab
    {
        public string Header { get; set; }
        public ObservableCollection<Entry> Data { get; set; } = new ObservableCollection<Entry>();
    }
}
