using dziennik_Admina.DataBaseAccess;
using dziennik_Admina.Models;
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
    /// Logika interakcji dla klasy AddEntry.xaml
    /// </summary>
    public partial class AddEntry : Window
    {
        JournalDbContext db;
        List<string> journals;
        string currrentUser;
        MainWindow mw;
        public AddEntry(JournalDbContext db, List<string> journals, string user, MainWindow mw)
        {
            InitializeComponent();
            this.db = db;
            this.journals = journals;
            this.JournalsComboBox.ItemsSource = this.journals;
            this.currrentUser = user;

            this.mw = mw;
        }
        public void AddEntryClick(object sender, RoutedEventArgs s)
        {
            TextBox entryTextBox = this.FindName("entryTextBox") as TextBox;
            if(entryTextBox.Text.Equals(""))
            {
                MessageBox.Show("Brak treści wpisu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            switch (this.JournalsComboBox.SelectedItem)
            {
                case ("JOURNAL1"):
                    Journal1 entry1 = new Journal1()
                    {
                        data = DateTime.Now,
                        username = currrentUser,
                        entry = entryTextBox.Text
                    };
                    db.Journal1.Add(entry1);
                    break;
                case ("JOURNAL2"):
                    Journal2 entry2 = new Journal2()
                    {
                        data = DateTime.Now,
                        username = currrentUser,
                        entry = entryTextBox.Text
                    };
                    db.Journal2.Add(entry2);
                    break;
                case ("JOURNAL3"):
                    Journal3 entry3 = new Journal3()
                    {
                        data = DateTime.Now,
                        username = currrentUser,
                        entry = entryTextBox.Text
                    };
                    db.Journal3.Add(entry3);
                    break;
                default:
                    MessageBox.Show("Wybierz odpowiedni dziennik", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            db.SaveChanges();
            mw.ShowJournal();
            this.Close();
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
