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
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        public User AuthorizatedUser { get; set; }
        public ObservableCollection<Report> Zayavki { get; set; }
        public Report SelectedReport { get; set; }
        public string SearchText { get; set; }

        // инициализация датагрида
        public Admin()
        {
            InitializeComponent();
            dgZayavki.Items.Clear();
            Zayavki = new ObservableCollection<Report>(dp_hus_dipEntities2.GetContext().Zayavka);
            dgZayavki.ItemsSource = Zayavki;
            DataContext = this;
        }

        // удаление выбранной заявки
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

        // обработчик события изменения текста
        private void tbPoisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateZayavki();
        }

        // обновление содержимого датагрида в зависимости от поискового текста
        private void UpdateZayavki()
        {
            Zayavki.Clear();
            foreach (var report in GetZayavki(SearchText?.ToLower() ?? string.Empty))
                Zayavki.Add(report);
        }

        // метод ищущий все заявки совпадающих содержимым с поисковым текстом
        private IEnumerable<Report> GetZayavki(string text)
        {
            return dp_hus_dipEntities2.GetContext().Zayavka
                .ToList()
                .Where(x => ($"{x.Date}{x.Type.Name}{x.Oboryduvanie.InventoryNumber}" +
                    $"{x.Opisanie}{x.MasterName}{x.ComentsFinishWork}{x.Status.Name}")
                    .ToLower()
                    .Contains(text));
        }
        
        // закрытие окна, показ окна авторизации
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Owner.Show();
        }

        // открытие окна редактирования заявки, если результат true обновляем данные в бд
        private void btnRedact_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReport is null)
            {
                MessageBox.Show("Выберите заявку!");
                return;
            }
            var dialogRes = new Redact(SelectedReport).ShowDialog();
            if (dialogRes == true)
            {
                var toUpdate = dp_hus_dipEntities2.GetContext().Zayavka.First(x => x.ID_Zayavka == SelectedReport.ID_Zayavka);
                toUpdate.Status = SelectedReport.Status;
                toUpdate.ComentsFinishWork = SelectedReport.ComentsFinishWork;
                toUpdate.MasterName = SelectedReport.MasterName;
                dp_hus_dipEntities2.GetContext().SaveChanges();
                UpdateZayavki();
            }
        }
    }
}
