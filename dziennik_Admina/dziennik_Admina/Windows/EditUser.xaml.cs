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
using System.Windows.Shapes;

namespace dziennik_Admina.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        JournalDbContext db;
        User user;
        List<Users_Roles> users_roles;
        MainWindow mw;
        public EditUser(JournalDbContext db, MainWindow mw)
        {
            InitializeComponent();
            this.db = db;
            this.mw = mw;
        }
        public void SearchClick(object sender, RoutedEventArgs s)
        {
            TextBox loginTextBox = this.FindName("loginTextBox") as TextBox;
            Label errorLoginLabel = this.FindName("errorLoginLabel") as Label;
            Button SaveButton = this.FindName("SaveButton") as Button;
            CheckBox Journal1CheckBox = this.FindName("Journal1CheckBox") as CheckBox;
            CheckBox Journal2CheckBox = this.FindName("Journal2CheckBox") as CheckBox;
            CheckBox Journal3CheckBox = this.FindName("Journal3CheckBox") as CheckBox;

            user = db.Users.FirstOrDefault(x => x.Username.Equals(loginTextBox.Text));
            if (user == null)
            {
                errorLoginLabel.Visibility = Visibility.Visible;
                Journal1CheckBox.IsChecked = false;
                Journal2CheckBox.IsChecked = false;
                Journal3CheckBox.IsChecked = false;
                SaveButton.IsEnabled = false;
            }
            else
            {
                if (errorLoginLabel.Visibility == Visibility.Visible)
                    errorLoginLabel.Visibility = Visibility.Collapsed;
                users_roles = db.Users_Roles.Where(x => x.ID_User == user.ID_User).ToList();
                List<Role> roles = new List<Role>();
                foreach(Users_Roles u in users_roles)
                {
                    roles.Add(db.Roles.FirstOrDefault(x => x.ID_Role == u.ID_Role));
                }
                foreach(Role r in roles)
                {
                    switch(r.Name)
                    {
                        case ("JOURNAL1"):
                            Journal1CheckBox.IsChecked = true;
                            break;
                        case ("JOURNAL2"):
                            Journal2CheckBox.IsChecked = true;
                            break;
                        case ("JOURNAL3"):
                            Journal3CheckBox.IsChecked = true;
                            break;
                    }
                }
                SaveButton.IsEnabled = true;
            }
        }
        public void SaveClick(object sender, RoutedEventArgs s)
        {
            Label errorPasswordLabel = this.FindName("errorPasswordLabel") as Label;
            Label errorLoginLabel = this.FindName("errorLoginLabel") as Label;
            if (passwordTextBox.Text.Equals(""))
            {
                errorPasswordLabel.Visibility = Visibility.Visible;
                return;
            }
            if(loginTextBox.Text.Equals(""))
            {
                errorLoginLabel.Visibility = Visibility.Visible;
                return;
            }

            
            var userEdit = db.Users.FirstOrDefault(x => x.Username.Equals(user.Username));
            userEdit.Password = ComputeHash(passwordTextBox.Text);
            db.SaveChanges();

            List<int> rolesID = new List<int>();
            foreach (Users_Roles u in users_roles)
            {
                db.Users_Roles.Remove(u);
            }
            db.SaveChanges();
            if ((bool)Journal1CheckBox.IsChecked)
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
            mw.ReloadComboBox();
            this.Close();
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
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
        }
    }
}
