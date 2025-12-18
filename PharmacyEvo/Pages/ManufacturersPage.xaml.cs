using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data;
using PharmacyEvo.Models;
using PharmacyEvo.Global;

namespace PharmacyEvo.Pages
{
    public partial class ManufacturersPage : Page
    {
        public ObservableCollection<Manufacturer> ManufacturersCollection { get; set; }
        private ObservableCollection<Manufacturer> _allManufacturers;
        public Manufacturer SelectedItem { get; set; }
        public bool IsAdmin { get; set; }

        public ManufacturersPage()
        {
            InitializeComponent();
            ManufacturersCollection = new ObservableCollection<Manufacturer>();
            _allManufacturers = new ObservableCollection<Manufacturer>();
            DataGrid.ItemsSource = ManufacturersCollection;
            IsAdmin = GlobalClass.CurrentUser?.IsAdmin ?? false;
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            _allManufacturers.Clear();
            ManufacturersCollection.Clear();
            var data = ProcedureDB.GetManufacturers();
            foreach (var item in data)
            {
                _allManufacturers.Add(item);
                ManufacturersCollection.Add(item);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new HomePage());
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTextBox.Text?.ToLower() ?? "";
            ManufacturersCollection.Clear();

            foreach (var manufacturer in _allManufacturers)
            {
                if (string.IsNullOrEmpty(searchText) ||
                    manufacturer.Name?.ToLower().Contains(searchText) == true)
                {
                    ManufacturersCollection.Add(manufacturer);
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            // Перемещаем пустой столбец в конец после загрузки всех столбцов
            if (DataGrid.Columns.Count > 0 && DataGrid.Columns[0] is DataGridTemplateColumn)
            {
                var emptyColumn = DataGrid.Columns[0];
                DataGrid.Columns.RemoveAt(0);
                emptyColumn.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                DataGrid.Columns.Add(emptyColumn);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame?.Navigate(new AddEditManufacturerPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                GlobalClass.MainFrame?.Navigate(new AddEditManufacturerPage(SelectedItem));
            }
            else
            {
                MessageBox.Show("Выберите запись для редактирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    ProcedureDB.DeleteManufacturer(SelectedItem.ManufacturerId);
                    LoadData();
                }
            }
        }
    }
}
