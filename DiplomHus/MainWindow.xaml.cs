using DiplomHus.Windows;
using OfficeOpenXml;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiplomHus
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            InitializeComponent();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            var queryUser = dp_hus_dipEntities2.GetContext()
                .User.FirstOrDefault(x => x.Login == txtLogin.Text && x.Password == pasPassword.Password);
            if (queryUser == null)
            {
                MessageBox.Show("Неверный логин или пароль!");
                return;
            }
            if (queryUser.Role.Name == "Admin")
            {
                // открываем окно админа

                var newAdmin = new AdminMenu { AuthorizatedUser = queryUser, Owner = this };
                newAdmin.Show();
                this.Hide();
            }
            else if (queryUser.Role.Name == "User")
            {
                var newWnd = new Zakaz(queryUser) { Owner = this };
                newWnd.Show();
                this.Hide();
                // открываем окно заказчика
            }
        }
    }
}
