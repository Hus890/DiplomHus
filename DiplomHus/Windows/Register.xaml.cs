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
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Patronomyc { get; set; }
        public Podrazdelenie SelectedPodrazdelenie { get; set; }
        public Role SelectedRole { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public List<Role> Roles { get; set; }
        public List<Podrazdelenie> Podrazdelenies { get; set; }

        public Register()
        {
            InitializeComponent();
            Roles = new List<Role>(dp_hus_dipEntities2.GetContext().Role);
            Podrazdelenies = new List<Podrazdelenie>(dp_hus_dipEntities2.GetContext().Podrazdelenie);
            DataContext = this;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!IsValidStrings(FirstName, SecondName, Patronomyc, Login, Password) 
                || SelectedRole == null
                || SelectedPodrazdelenie == null)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }
            var newUser = new User
            {
                Login = Login,
                Password = Password,
                Name = FirstName,
                Surname = SecondName,
                Patronomyc = Patronomyc,
                Role = SelectedRole,
                Podrazdelenie = SelectedPodrazdelenie
            };
            dp_hus_dipEntities2.GetContext().User.Add(newUser);
            dp_hus_dipEntities2.GetContext().SaveChanges();
            DialogResult = true;
        }

        private bool IsValidStrings(params string[] strings)
        {
            return !strings.All(string.IsNullOrEmpty);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();    
        }
    }
}
