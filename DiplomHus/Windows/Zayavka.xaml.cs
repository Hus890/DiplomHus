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

using Request = DiplomHus.Zayavka;

namespace DiplomHus.Windows
{
    /// <summary>
    /// Логика взаимодействия для Zayavka.xaml
    /// </summary>
    public partial class Zayavka : Window
    {
        public User User { get; set; }

        // инициализация типа и инвентарного номера
        public Zayavka()
        {
            InitializeComponent();
            cmbType.ItemsSource = dp_hus_dipEntities2.GetContext().Type
                .Select(x => x.Name)
                .ToList();
            tbNumerZayavka.Text = ((dp_hus_dipEntities2.GetContext().Zayavka
                .ToList()
                .LastOrDefault()?.ID_Zayavka ?? 0) + 1).ToString();
        }

        // создание заявки
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var oborudovanie = dp_hus_dipEntities2
                .GetContext()
                .Oboryduvanie
                .FirstOrDefault(x => x.InventoryNumber == tbInvetoryNumer.Text);
            if (oborudovanie == null)
            {
                MessageBox.Show("нет табеля");
                return;
            }
            var mestoRemonta = dp_hus_dipEntities2
                .GetContext()
                .MestoRemonta
                .FirstOrDefault(x => x.Korpus == tbKorpus.Text && x.Floor == tbFloor.Text && x.Room == tbRoom.Text);
            var type = dp_hus_dipEntities2
                .GetContext()
                .Type
                .First(x => x.Name == cmbType.SelectedValue.ToString());
            var newZayavka = new Request
            {
                User = User,
                Oboryduvanie = oborudovanie,
                Phone = tbNumber.Text,
                Opisanie = tbOpisanie.Text,
                Status = dp_hus_dipEntities2.GetContext().Status.First(x => x.Name == "Новый"),
                Type = type,
                Date = DateTime.Now,
                MestoRemonta = mestoRemonta ?? new MestoRemonta
                {
                    Floor = tbFloor.Text,
                    Room = tbRoom.Text,
                    Korpus = tbKorpus.Text,
                    Podrazdelenie = User.Podrazdelenie
                },
                ComentsFinishWork = string.Empty,
                MasterName = string.Empty
            };
            dp_hus_dipEntities2.GetContext().Zayavka.Add(newZayavka);
            dp_hus_dipEntities2.GetContext().SaveChanges();
            DialogResult = true;
            Close();
        }

        // метод ищет соответствующее имя оборудования по его инвертарному номеру
        private void tbInvetoryNumer_TextChanged(object sender, TextChangedEventArgs e)
        {
            var oborudovatie = dp_hus_dipEntities2
                .GetContext()
                .Oboryduvanie
                .FirstOrDefault(x => x.InventoryNumber == tbInvetoryNumer.Text);
            tborodyduvanie.Text = oborudovatie != null ? oborudovatie.Name : string.Empty;
        }

        // инициализация подразделения и фио в шапке окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbPodrazdelenie.Text = User.Podrazdelenie.Name;
            tbZakazchik.Text = $"{User.Surname} {User.Name} {User.Patronomyc}";
        }
    }
}
