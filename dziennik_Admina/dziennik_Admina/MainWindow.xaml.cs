using dziennik_Admina.DataBaseAccess;
using dziennik_Admina.Models;
using dziennik_Admina.TabItems;
using dziennik_Admina.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace dziennik_Admina
{
    public partial class MainWindow : Window
    {
        private bool USER_LOGGED_IN = false;
        private JournalDbContext db;
        private string _username;
        private List<string> roles = new List<string>();
        List<Entry> entries;
        string journal;

        public MainWindow()
        {
            InitializeComponent();
            db = new JournalDbContext();
        }
        public void LoginButton(object sender, RoutedEventArgs s)
        {

            if (!USER_LOGGED_IN)
            {
                LogIn(this.loginTextBox.Text, this.passwordTextBox.Password);                
            }
            else
            {
                if (db.Users.FirstOrDefault(x => x.Username.Equals(_username)).IsAdmin == true)     //guziki widoczne tylko dla adminów
                {
                    this.removeUserButton.Visibility = Visibility.Collapsed;
                    this.addUserButton.Visibility = Visibility.Collapsed;
                    this.editUserButton.Visibility = Visibility.Collapsed;
                }
                _username = "";
                roles.Clear();
                this.Journals.ItemsSource = null;
                this.Wpisy.ItemsSource = null;
                USER_LOGGED_IN = false;
                ReverseVisibilityLoginButtons();
                this.addNoteButton.IsEnabled = false;

            }
        }
        public void AddUserClick (object sender, RoutedEventArgs s)
        {
            var UserCreateWindow = new AddUser(db, this);
            UserCreateWindow.Show();
        }
        public void RemoveUserClick(object sender, RoutedEventArgs s)
        {
            var UserRemoveWindow = new RemoveUser(db);
            UserRemoveWindow.Show();
        }
        public void AddEntryClick(object sender, RoutedEventArgs s)
        {
            var AddEntryWindow = new AddEntry(db, roles, _username, this);
            AddEntryWindow.Show();
        }
        public void EditUserClick(object sender, RoutedEventArgs s)
        {
            var EditUserWindow = new EditUser(db, this);
            EditUserWindow.Show();
        }
        public void LogIn(string login, string password)
        {

            if(this.loginTextBox.Text=="")
            {
                MessageBox.Show("Blędny login", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var user = db.Users.FirstOrDefault(x => x.Username == this.loginTextBox.Text);
            if (this.loginTextBox.Text != "" && user != null && user.Username == this.loginTextBox.Text)
            {
                User userFromDb = db.Users.FirstOrDefault(x => x.Username == login);
                if(ComputeHash(password) == userFromDb.Password)
                {
                    _username = user.Username;
                    List<Users_Roles> users_roles = db.Users_Roles.Where(x => x.ID_User == user.ID_User).ToList();
                    foreach(Users_Roles row in users_roles)
                    {
                        var role = db.Roles.FirstOrDefault(x => x.ID_Role == row.ID_Role);
                        roles.Add(role.Name);
                    }

                    ReverseVisibilityLoginButtons();
                    if (roles.Count() != 0)
                    {
                        this.Journals.SelectedItem = roles[0];
                        this.addNoteButton.IsEnabled = true;
                        this.Journals.IsEnabled = true;
                        this.LoadJournal.IsEnabled = true;
                        this.Wpisy.IsEnabled = true;
                    }
                    if (db.Users.FirstOrDefault(x => x.Username.Equals(_username)).IsAdmin == true)
                    {
                        this.removeUserButton.Visibility = Visibility.Visible;
                        this.addUserButton.Visibility = Visibility.Visible;
                        this.editUserButton.Visibility = Visibility.Visible;
                    }

                    this.Journals.ItemsSource = roles;

                    USER_LOGGED_IN = true;
                }
                else
                    MessageBox.Show("Blędne hasło", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            else
                MessageBox.Show("Blędny login", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }   
        public void ShowJournalClick(object sender, RoutedEventArgs s)
        {
            ShowJournal();
        }
        public void ShowJournal()
        {
            entries = new List<Entry>();
            this.Wpisy.ItemsSource = null;
            journal = this.Journals.SelectedItem.ToString();
            bool isUserAdmin = db.Users.FirstOrDefault(x => x.Username.Equals(_username)).IsAdmin;
            switch (journal)
            {
                case ("JOURNAL1"):
                    var entriesFromDb1 = db.Journal1;
                    foreach(Journal1 j in entriesFromDb1)
                    {
                        entries.Add(new Entry()
                        {
                            ID = j.ID,
                            Date = j.data,
                            Username = j.username,
                            Text = j.entry,
                            isButtonEnabled = (j.username == _username || isUserAdmin) 
                        });
                    }
                    break;
                case ("JOURNAL2"):
                    var entriesFromDb2 = db.Journal2;
                    foreach (Journal2 j in entriesFromDb2)
                    {
                        entries.Add(new Entry()
                        {
                            ID = j.ID,
                            Date = j.data,
                            Username = j.username,
                            Text = j.entry,
                            isButtonEnabled = (j.username == _username || isUserAdmin)
                        });
                    }
                    break;
                case ("JOURNAL3"):
                    var entriesFromDb3 = db.Journal3;
                    foreach (Journal3 j in entriesFromDb3)
                    {
                        entries.Add(new Entry()
                        {
                            ID = j.ID,
                            Date = j.data,
                            Username = j.username,
                            Text = j.entry,
                            isButtonEnabled = (j.username == _username || isUserAdmin)
                        });
                    }
                    break;
            }
            this.Wpisy.ItemsSource = entries;
        }
        public void ReloadComboBox()
        {
            
            roles = new List<string>();
            var userFromDb = db.Users.FirstOrDefault(x => x.Username == _username);
            List<Users_Roles> users_roles = db.Users_Roles.Where(x => x.ID_User == userFromDb.ID_User).ToList();
            foreach (Users_Roles row in users_roles)
            {
                var role = db.Roles.FirstOrDefault(x => x.ID_Role == row.ID_Role);
                roles.Add(role.Name);
            }
            this.Journals.ItemsSource = roles;
            if (roles.Count() != 0)
            {
                this.Journals.SelectedItem = roles[0];
                this.addNoteButton.IsEnabled = true;
                this.Journals.IsEnabled = true;
                this.LoadJournal.IsEnabled = true;
                this.Wpisy.IsEnabled = true;
            }
            else
            {
                this.Journals.IsEnabled = false;
                this.LoadJournal.IsEnabled = false;
                this.Wpisy.IsEnabled = false;
            }
            this.Wpisy.ItemsSource = null;
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

        public void ReverseVisibilityLoginButtons()
        {

            if (loginTextBox.IsVisible)
            {
                this.loginTextBox.Visibility = Visibility.Collapsed;
                this.loginTextBox.Text = "";
                this.passwordTextBox.Visibility = Visibility.Collapsed;
                this.passwordTextBox.Password = "";
                this.loginLabel.Visibility = Visibility.Collapsed;
                this.passwordLabel.Visibility = Visibility.Collapsed;
                this.greeting.Content = "Witaj " + _username;
                this.greeting.Visibility = Visibility.Visible;
                this.loginButton.Content = "Wyloguj";
                //this.LoadJournal.IsEnabled = true;
                //this.Journals.IsEnabled = true;
                //this.Wpisy.IsEnabled = true;
            }
                
            else
            {
                this.loginTextBox.Visibility = Visibility.Visible;
                this.passwordTextBox.Visibility = Visibility.Visible;
                this.loginLabel.Visibility = Visibility.Visible;
                this.passwordLabel.Visibility = Visibility.Visible;
                this.greeting.Content ="";
                this.greeting.Visibility = Visibility.Collapsed;
                this.loginButton.Content = "Zaloguj";
                //this.LoadJournal.IsEnabled = false;
                //this.Journals.IsEnabled = false;
                //this.Wpisy.IsEnabled = false;
            }
        }
        
        public void EditButtonClick(object sender, RoutedEventArgs s)
        {
            int ID = (int)((Button)sender).CommandParameter;
            Entry entry = entries.FirstOrDefault(x => x.ID == ID);
            EditEntry editEntryWindow = new EditEntry(db, this, entry, journal);
            editEntryWindow.Show();
        }
        public void DeleteButtonClick(object sender, RoutedEventArgs s)
        {
            int ID = (int)((Button)sender).CommandParameter;
            switch (journal)
            {
                case ("JOURNAL1"):
                    var entry1 = db.Journal1.FirstOrDefault(x => x.ID == ID);
                    db.Journal1.Remove(entry1);
                    break;
                case ("JOURNAL2"):
                    var entry2 = db.Journal2.FirstOrDefault(x => x.ID == ID);
                    db.Journal2.Remove(entry2);
                    break;
                case ("JOURNAL3"):
                    var entry3 = db.Journal3.FirstOrDefault(x => x.ID == ID);
                    db.Journal3.Remove(entry3);
                    break;
            }
            db.SaveChanges();
            ShowJournal();
        }
    }
}
