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
    /// Логика взаимодействия для Zakaz.xaml
    /// </summary>
    public partial class Zakaz : Window
    {
        public User AuthorizatedUser { get; set; }

        public Zakaz()
        {
            InitializeComponent();
        }

        private void btnCreateZayavka_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new Zayavka { User = AuthorizatedUser };
            wnd.ShowDialog();
        }
    }
}
