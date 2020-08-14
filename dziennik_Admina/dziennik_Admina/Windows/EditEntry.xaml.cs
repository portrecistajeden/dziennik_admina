using dziennik_Admina.DataBaseAccess;
using dziennik_Admina.TabItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dziennik_Admina.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy EditEntry.xaml
    /// </summary>
    public partial class EditEntry : Window
    {
        Entry entry;
        JournalDbContext db;
        MainWindow mw;
        string journal;
        public EditEntry(JournalDbContext db, MainWindow mw, Entry entry, string journal)
        {
            InitializeComponent();
            this.db = db;
            this.mw = mw;
            this.entry = entry;
            this.journal = journal;
            this.entryTextBox.Text = entry.Text;
        }
        public void EditEntryClick(object sender, RoutedEventArgs s)
        {

            switch(journal)
            {
                case ("JOURNAL1"):
                    var entryFromDb1 = db.Journal1.FirstOrDefault(x => x.ID == entry.ID);
                    entryFromDb1.entry = this.entryTextBox.Text;
                    break;
                case ("JOURNAL2"):
                    var entryFromDb2 = db.Journal2.FirstOrDefault(x => x.ID == entry.ID);
                    entryFromDb2.entry = this.entryTextBox.Text;
                    break;
                case ("JOURNAL3"):
                    var entryFromDb3 = db.Journal3.FirstOrDefault(x => x.ID == entry.ID);
                    entryFromDb3.entry = this.entryTextBox.Text;
                    break;
            }
            db.SaveChanges();
            mw.ShowJournal();
            this.Close();
        }
    }
}
