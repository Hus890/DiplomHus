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
    /// Логика взаимодействия для Redact.xaml
    /// </summary>
    public partial class Redact : Window
    {
        // Свойства к которым биндятся наши контролы
        public List<Status> Statuses { get; set; }
        public Status SelectedStatus { get; set; }
        public string CommentsFinishWork { get; set; }
        public DiplomHus.Zayavka Zayavka { get; set; }

        // инициализация контролов на окне значениями из редактируемой заявки
        public Redact(DiplomHus.Zayavka zayavka)
        {
            InitializeComponent();
            Statuses = new List<Status>(dp_hus_dipEntities2.GetContext().Status);
            cmbStatus.Items.Clear();
            cmbStatus.ItemsSource = Statuses;
            Zayavka = zayavka;
            SelectedStatus = Statuses.First(x => x.ID_Status == Zayavka.Status.ID_Status);
            DataContext = this;
        }

        // Кнопка ОК
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Zayavka.Status = SelectedStatus;
            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
