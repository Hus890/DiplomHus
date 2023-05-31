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
    /// Логика взаимодействия для AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        public User AuthorizatedUser { get; set; }

        public AdminMenu()
        {
            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            var mainWnd = Owner as MainWindow;
            mainWnd.txtLogin.Clear();
            mainWnd.pasPassword.Clear();
            Owner.Show();
        }

        private void btnProsmort_Click(object sender, RoutedEventArgs e)
        {
            var newAdmin = new Admin { AuthorizatedUser = AuthorizatedUser, Owner = this };
            newAdmin.Show();
            this.Hide();
        }

        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            Window wndReg = new Register();
            var result = wndReg.ShowDialog();
            if (result == true)
            {
                MessageBox.Show("Пользователь зарегистрирован");
            }
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            Window wndUser = new Users();
            wndUser.ShowDialog();
        }
    }
}
