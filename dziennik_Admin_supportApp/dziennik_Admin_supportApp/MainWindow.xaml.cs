using dziennik_Admina.DataBaseAccess;
using dziennik_Admina.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dziennik_Admin_supportApp
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        JournalDbContext db;
        User user;
        List<Users_Roles> users_roles;
        public MainWindow()
        {
            InitializeComponent();
            db = new JournalDbContext();
        }

        public void CreateClick(object sender, RoutedEventArgs s)
        {
            if (!this.loginTextBox.Text.Equals(""))
            {
                if (db.Users.FirstOrDefault(x => x.Username.Equals(this.loginTextBox.Text)) == null)
                {
                    if (this.passwordTextBox.Text.Equals(""))
                    {
                        MessageBox.Show("Brak hasła", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    var user = new User
                    {
                        Username = this.loginTextBox.Text,
                        Password = ComputeHash(this.passwordTextBox.Text)
                    };
                    List<int> rolesID = new List<int>();
                    if ((bool)this.Journal1CheckBox.IsChecked)
                    {
                        var role = db.Roles.FirstOrDefault(x => x.Name.Equals("JOURNAL1"));
                        rolesID.Add(role.ID_Role);
                    }
                    if ((bool)this.Journal2CheckBox.IsChecked)
                    {
                        var role = db.Roles.FirstOrDefault(x => x.Name.Equals("JOURNAL2"));
                        rolesID.Add(role.ID_Role);
                    }
                    if ((bool)this.Journal3CheckBox.IsChecked)
                    {
                        var role = db.Roles.FirstOrDefault(x => x.Name.Equals("JOURNAL3"));
                        rolesID.Add(role.ID_Role);
                    }

                    if ((bool)this.isAdmin.IsChecked)
                        user.IsAdmin = true;
                    else
                        user.IsAdmin = false;

                    db.Users.Add(user);
                    db.SaveChanges();
                    user = db.Users.FirstOrDefault(x => x.Username.Equals(this.loginTextBox.Text));

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

        public void SearchClick(object sender, RoutedEventArgs s)
        {
            this.passwordTextBox.Text = "";
            user = db.Users.FirstOrDefault(x => x.Username.Equals(this.loginTextBox.Text));
            if (user == null)
            {
                this.Journal1CheckBox.IsChecked = false;
                this.Journal2CheckBox.IsChecked = false;
                this.Journal3CheckBox.IsChecked = false;
                this.EditButton.IsEnabled = false;
            }
            else
            {
                users_roles = db.Users_Roles.Where(x => x.ID_User == user.ID_User).ToList();
                List<Role> roles = new List<Role>();
                foreach (Users_Roles u in users_roles)
                {
                    roles.Add(db.Roles.FirstOrDefault(x => x.ID_Role == u.ID_Role));
                }
                foreach (Role r in roles)
                {
                    switch (r.Name)
                    {
                        case ("JOURNAL1"):
                            this.Journal1CheckBox.IsChecked = true;
                            break;
                        case ("JOURNAL2"):
                            this.Journal2CheckBox.IsChecked = true;
                            break;
                        case ("JOURNAL3"):
                            this.Journal3CheckBox.IsChecked = true;
                            break;
                    }
                }
                EditButton.IsEnabled = true;
            }
        }
        public void SaveClick(object sender, RoutedEventArgs s)
        {
            if (this.passwordTextBox.Text.Equals(""))
            {
                MessageBox.Show("Brak hasła", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            var userEdit = db.Users.FirstOrDefault(x => x.Username.Equals(user.Username));
            userEdit.Password = ComputeHash(this.passwordTextBox.Text);
            db.SaveChanges();

            List<int> rolesID = new List<int>();
            foreach (Users_Roles u in users_roles)
            {
                db.Users_Roles.Remove(u);
            }
            db.SaveChanges();
            if ((bool)this.Journal1CheckBox.IsChecked)
            {
                var role = db.Roles.FirstOrDefault(x => x.Name.Equals("JOURNAL1"));
                rolesID.Add(role.ID_Role);
            }
            if ((bool)this.Journal2CheckBox.IsChecked)
            {
                var role = db.Roles.FirstOrDefault(x => x.Name.Equals("JOURNAL2"));
                rolesID.Add(role.ID_Role);
            }
            if ((bool)this.Journal3CheckBox.IsChecked)
            {
                var role = db.Roles.FirstOrDefault(x => x.Name.Equals("JOURNAL3"));
                rolesID.Add(role.ID_Role);
            }
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
            MessageBox.Show("Zmieniono dane użytkownika", "Powodzenie", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public void RemoveClick(object sender, RoutedEventArgs s)
        {
            if (db.Users.FirstOrDefault(x => x.Username.Equals(this.loginTextBox.Text)).IsAdmin == true)
            {
                MessageBox.Show("Nie można usunąć admina", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!this.loginTextBox.Text.Equals(""))
            {
                var user = db.Users.FirstOrDefault(x => x.Username.Equals(this.loginTextBox.Text));
                if (user != null)
                {
                    db.Users.Remove(user);
                    List<Users_Roles> users_Roles = db.Users_Roles.Where(x => x.ID_User == user.ID_User).ToList();
                    foreach (Users_Roles u in users_Roles)
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
