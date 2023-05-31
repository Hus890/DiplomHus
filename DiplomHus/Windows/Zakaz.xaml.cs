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

using Report = DiplomHus.Zayavka;

namespace DiplomHus.Windows
{
    /// <summary>
    /// Логика взаимодействия для Zakaz.xaml
    /// </summary>
    public partial class Zakaz : Window
    {
        public User AuthorizatedUser { get; set; }
        public ObservableCollection<Report> Zayavki { get; set; }
        public Report SelectedReport { get; set; }
        public string SearchText { get; set; }

        // Метод создания окна - инициализация содержимого
        public Zakaz(User user)
        {
            InitializeComponent();
            AuthorizatedUser = user;
            Zayavki = new ObservableCollection<Report>(dp_hus_dipEntities2.GetContext().Zayavka.ToList().Where(x => x.User == AuthorizatedUser));
            dgZayavki.Items.Clear();
            dgZayavki.ItemsSource = Zayavki;
            DataContext = this;
        }

        // переход на окно формирования заявки
        private void btnCreateZayavka_Click(object sender, RoutedEventArgs e)
        {
            var wnd = new Zayavka { User = AuthorizatedUser };
            var result = wnd.ShowDialog();
            if (result == true)
            {
                UpdateZayavki();
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            var mainWnd = Owner as MainWindow;
            mainWnd.txtLogin.Clear();
            mainWnd.pasPassword.Clear();
            Owner.Show();
        }

        // Удаление выбранного элемента
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReport != null)
            {
                dp_hus_dipEntities2.GetContext().Zayavka.Remove(SelectedReport);
                dp_hus_dipEntities2.GetContext().SaveChanges();
                Zayavki.Remove(SelectedReport);
            }
            else MessageBox.Show("Выберите заявку");
        }

        private void tbPoisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Поиск 
            UpdateZayavki();
        }

        private void UpdateZayavki()
        {
            Zayavki.Clear();
            foreach (var report in GetZayavki(SearchText?.ToLower() ?? string.Empty))Zayavki.Add(report);
        }

        // Берет текст и находит совпадения из бд
        private IEnumerable<Report> GetZayavki(string text)
        {
            return dp_hus_dipEntities2.GetContext().Zayavka
                .ToList()
                .Where(x => x.User == AuthorizatedUser)
                .Where(x => $"{x.Date}{x.Type.Name}{x.Oboryduvanie.InventoryNumber}{x.Opisanie}".ToLower().Contains(text));
        }

        private void tbFio_TextChanged(object sender, TextChangedEventArgs e)
        {
            //-------------------------------
        }
    }
}
