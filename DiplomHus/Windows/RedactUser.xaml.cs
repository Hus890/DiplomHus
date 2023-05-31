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

namespace DiplomHus.Windows
{
    /// <summary>
    /// Логика взаимодействия для RedactUser.xaml
    /// </summary>
    public partial class RedactUser : Window
    {
        public Podrazdelenie SelectedPodrazdelenie { get; set; }
        public Role SelectedRole { get; set; }

        public List<Role> Roles { get; set; }
        public List<Podrazdelenie> Podrazdelenies { get; set; }

        public User EditUser { get; set; }

        public RedactUser(User user)
        {
            EditUser = user;
            InitializeComponent();
            Roles = new List<Role>(dp_hus_dipEntities2.GetContext().Role);
            Podrazdelenies = new List<Podrazdelenie>(dp_hus_dipEntities2.GetContext().Podrazdelenie);
            DataContext = this;
            SelectedRole = user.Role;
            SelectedPodrazdelenie = user.Podrazdelenie;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidStrings(EditUser.Login, EditUser.Password, EditUser.Name, EditUser.Surname, EditUser.Patronomyc))
            {
                MessageBox.Show("Заполните все поля");
                return; 
            }
            EditUser.Role = SelectedRole;
            EditUser.Podrazdelenie = SelectedPodrazdelenie;
            dp_hus_dipEntities2.GetContext().SaveChanges();
            DialogResult = true;
        }

        private bool IsValidStrings(params string[] strings)
        {
            return !strings.All(string.IsNullOrEmpty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
