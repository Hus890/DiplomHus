using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
//Привет
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
            cmbStatus.ItemsSource = dp_hus_dipEntities2.GetContext().Status.ToList();
            cmbMaster.ItemsSource = dp_hus_dipEntities2.GetContext().User
                .Where(x => x.Role.Name == "Master")
                .ToList();
            dgZayavki.Items.Clear();
            Zayavki = new ObservableCollection<Report>(dp_hus_dipEntities2.GetContext().Zayavka);
            dgZayavki.ItemsSource = Zayavki;
            dgZayavki.SelectedIndex = Zayavki.Any() ? 0 : -1;
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
            foreach (var report in GetZayavki(SearchText?.ToLower() ?? string.Empty))Zayavki.Add(report);
        }

        // метод ищущий все заявки совпадающих содержимым с поисковым текстом
        private IEnumerable<Report> GetZayavki(string text)
        {
            return dp_hus_dipEntities2.GetContext().Zayavka.ToList().Where(x => ($"{x.Date}{x.Type.Name}{x.Oboryduvanie.InventoryNumber}" +$"{x.Opisanie}{x.MasterName}{x.ComentsFinishWork}{x.Status.Name}") .ToLower().Contains(text));
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

        private void BtnPolzovatel_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Register();
            window.ShowDialog();
        }
        //Запуск Exelw
        private void btnOtchet_Click(object sender, object e)
        {
            var dialog = new SaveFileDialog { DefaultExt = "xlsx" };
            var res = dialog.ShowDialog(this);
            if (res == true)
            {
                CreateExcelReport(Zayavki, dialog.FileName);
                MessageBox.Show("Отчет сохранен");
            }
        }

        private void CreateExcelReport(Collection<Report> zayavkas, string savePath)
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Отчет");

                // Заголовки столбцов
                worksheet.Cells[1, 1].Value = "Номер Заявки";
                worksheet.Cells[1, 2].Value = "Номер Пользователя";
                worksheet.Cells[1, 3].Value = "Тип ремонта";
                worksheet.Cells[1, 4].Value = "Инвентарный номер";
                worksheet.Cells[1, 5].Value = "Местоположение";
                worksheet.Cells[1, 6].Value = "Статус";
                worksheet.Cells[1, 7].Value = "Телефон";
                worksheet.Cells[1, 8].Value = "Дата";
                worksheet.Cells[1, 9].Value = "Описание";
                worksheet.Cells[1, 10].Value = "Комментарий";
                worksheet.Cells[1, 11].Value = "Мастер";

                // Данные
                int row = 2;
                foreach (Report zayavka in zayavkas)
                {
                    worksheet.Cells[row, 1].Value = zayavka.ID_Zayavka;
                    worksheet.Cells[row, 2].Value = zayavka.ID_User;
                    worksheet.Cells[row, 3].Value = zayavka.ID_Type;
                    worksheet.Cells[row, 4].Value = zayavka.ID_Oboryduvanie;
                    worksheet.Cells[row, 5].Value = zayavka.ID_MestoRemonta;
                    worksheet.Cells[row, 6].Value = zayavka.ID_Status;
                    worksheet.Cells[row, 7].Value = zayavka.Phone;
                    worksheet.Cells[row, 8].Value = $"{zayavka.Date:dd.MM.yyyy}" ;
                    worksheet.Cells[row, 9].Value = zayavka.Opisanie;
                    worksheet.Cells[row, 10].Value = zayavka.ComentsFinishWork;
                    worksheet.Cells[row, 11].Value = zayavka.MasterName;

                    row++;
                }

                // Автонастройка ширины столбцов
                worksheet.Cells.AutoFitColumns();

                // Сохранение файла
                FileInfo fileInfo = new FileInfo(savePath);
                excelPackage.SaveAs(fileInfo);
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void dgZayavki_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbStatus.SelectedItem = (cmbStatus.ItemsSource as IEnumerable<Status>).FirstOrDefault(x => x.ID_Status == SelectedReport?.ID_Status);
            cmbMaster.SelectedItem = (cmbMaster.ItemsSource as IEnumerable<User>).FirstOrDefault(x => x.ID_User == SelectedReport?.MasterName);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedReport == null)
            {
                MessageBox.Show("Выберите заявку");
                return;
            }

            SelectedReport.Status = cmbStatus.SelectedItem as Status;
            SelectedReport.MasterName = (cmbMaster.SelectedItem as User).ID_User;
            dp_hus_dipEntities2.GetContext().SaveChanges();
            UpdateZayavki();
        }
    }
}
