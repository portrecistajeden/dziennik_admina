using dziennik_Admina.DataBaseAccess;
using dziennik_Admina.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public partial class RemoveUser : Window
    {
        JournalDbContext db;
        string _username;
        public RemoveUser(JournalDbContext db, string _username)
        {
            InitializeComponent();
            this.db = db;
            this._username = _username;
        }
        public void RemoveUserClick (object sender, RoutedEventArgs s)
        {
            if(db.Users.FirstOrDefault(x => x.Username.Equals(_username)).IsAdmin == true)
            {
                MessageBox.Show("Nie można usunąć admina", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(!this.loginTextBox.Text.Equals(""))
            { 
                var user = db.Users.FirstOrDefault(x => x.Username.Equals(this.loginTextBox.Text));
                if(user != null)
                {
                    db.Users.Remove(user);
                    List<Users_Roles> users_Roles = db.Users_Roles.Where(x => x.ID_User == user.ID_User).ToList();
                    foreach(Users_Roles u in users_Roles)
                    {
                        db.Users_Roles.Remove(u);
                    }
                    db.SaveChanges();
                    MessageBox.Show("Usunieto użytkownika " + user.Username, "Usunięto", MessageBoxButton.OK, MessageBoxImage.Information);
                    //this.Close();
                }
                else
                    MessageBox.Show("Nie ma takiego użytkownika", "Brak użytkownika", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Wpisz nazwę użytkownika", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
