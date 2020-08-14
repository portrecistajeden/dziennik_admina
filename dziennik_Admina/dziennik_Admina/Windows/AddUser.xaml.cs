using dziennik_Admina.DataBaseAccess;
using dziennik_Admina.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
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
    /// Logika interakcji dla klasy AddUser.xaml
    /// </summary>
    public partial class AddUser : Window
    {
        JournalDbContext db;
        MainWindow mw;
        public AddUser(JournalDbContext db, MainWindow mw)
        {
            InitializeComponent();
            this.db = db;
            this.mw = mw;
        }
        public void AddUserClick (object sender, RoutedEventArgs s)
        {
            TextBox loginTextBox = this.FindName("loginTextBox") as TextBox;
            TextBox passwordTextBox = this.FindName("passwordTextBox") as TextBox;

            CheckBox Journal1CheckBox = this.FindName("Journal1CheckBox") as CheckBox;
            CheckBox Journal2CheckBox = this.FindName("Journal2CheckBox") as CheckBox;
            CheckBox Journal3CheckBox = this.FindName("Journal3CheckBox") as CheckBox;

            if (!loginTextBox.Text.Equals(""))
            {
                if (db.Users.FirstOrDefault(x => x.Username.Equals(loginTextBox.Text)) == null)
                { 
                    if(passwordTextBox.Text.Equals(""))
                    {
                        MessageBox.Show("Brak hasła", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var user = new User
                    {
                        Username = loginTextBox.Text,
                        Password = ComputeHash(passwordTextBox.Text)
                    };
                    List<int> rolesID = new List<int>();
                    if((bool)Journal1CheckBox.IsChecked)
                    {
                        var role = db.Roles.FirstOrDefault(x => x.Name.Equals("JOURNAL1"));
                        rolesID.Add(role.ID_Role);
                    }
                    if ((bool)Journal2CheckBox.IsChecked)
                    {
                        var role = db.Roles.FirstOrDefault(x => x.Name.Equals("JOURNAL2"));
                        rolesID.Add(role.ID_Role);
                    }
                    if ((bool)Journal3CheckBox.IsChecked)
                    {
                        var role = db.Roles.FirstOrDefault(x => x.Name.Equals("JOURNAL3"));
                        rolesID.Add(role.ID_Role);
                    }
                    db.Users.Add(user);
                    db.SaveChanges();
                    user = db.Users.FirstOrDefault(x => x.Username.Equals(loginTextBox.Text));
                
                    foreach (int id in rolesID)
                    {
                        var user_role = new Users_Roles
                        {
                            ID_User = user.ID_User,
                            ID_Role = id
                        };
                        db.Users_Roles.Add(user_role);
                    }

                    db.SaveChanges();
                    MessageBox.Show("Dodano użytkownika " + user.Username, "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
                    //this.Close();
                }
                else
                    MessageBox.Show("Taki użytkownik już istnieje", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
                MessageBox.Show("Brak nazwy użytkownika", "Bląd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public string ComputeHash(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
 
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    stringBuilder.Append(bytes[i].ToString("x2"));
                }
                return stringBuilder.ToString();
            }
        }
        

    }
}
