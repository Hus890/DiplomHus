using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DiplomHus.Windows
{
    /// <summary>
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        public ObservableCollection<User> UsersCollection { get; set; }
        public User SelectedUser { get; set; }

        public Users()
        {
            InitializeComponent();
            UsersCollection = new ObservableCollection<User>(dp_hus_dipEntities2.GetContext().User);
            DataContext = this;
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUser != null)
            {
                dp_hus_dipEntities2.GetContext().User.Remove(SelectedUser);
                dp_hus_dipEntities2.GetContext().SaveChanges();
                UsersCollection.Remove(SelectedUser);
            }
            else MessageBox.Show("Выберите пользователя");
        }

        private void btnRedact_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedUser != null)
            {
                Window wndRedact = new RedactUser(SelectedUser);
                var res = wndRedact.ShowDialog();
                if (res == true)
                {
                    UsersCollection.Clear();
                    foreach (var u in dp_hus_dipEntities2.GetContext().User)
                        UsersCollection.Add(u);
                }
            }
            else MessageBox.Show("Выберите пользователя");
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
