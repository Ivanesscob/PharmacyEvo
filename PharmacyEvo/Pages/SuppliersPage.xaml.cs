using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class SuppliersPage : Page
    {
        public ObservableCollection<Supplier> SuppliersCollection { get; set; }
        private ObservableCollection<Supplier> _allSuppliers;
        public Supplier SelectedItem { get; set; }
        public bool IsAdmin { get; set; }

        public SuppliersPage()
        {
            InitializeComponent();
            SuppliersCollection = new ObservableCollection<Supplier>();
            _allSuppliers = new ObservableCollection<Supplier>();
            DataGrid.ItemsSource = SuppliersCollection;
            IsAdmin = GlobalClass.CurrentUser?.IsAdmin ?? false;
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            _allSuppliers.Clear();
            SuppliersCollection.Clear();
            var data = ProcedureDB.GetSuppliers();
            foreach (var item in data)
            {
                _allSuppliers.Add(item);
                SuppliersCollection.Add(item);
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
            SuppliersCollection.Clear();

            foreach (var supplier in _allSuppliers)
            {
                if (string.IsNullOrEmpty(searchText) ||
                    supplier.Name?.ToLower().Contains(searchText) == true ||
                    supplier.Phone?.ToLower().Contains(searchText) == true ||
                    supplier.Address?.ToLower().Contains(searchText) == true)
                {
                    SuppliersCollection.Add(supplier);
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
            GlobalClass.MainFrame?.Navigate(new AddEditSupplierPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                GlobalClass.MainFrame?.Navigate(new AddEditSupplierPage(SelectedItem));
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
                    ProcedureDB.DeleteSupplier(SelectedItem.SupplierId);
                    LoadData();
                }
            }
        }
    }
}
