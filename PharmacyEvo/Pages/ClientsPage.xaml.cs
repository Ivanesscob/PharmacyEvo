using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ObservableCollection<Customer> ClientsCollection { get; set; }
        private ObservableCollection<Customer> _allCustomers;
        public Customer SelectedItem { get; set; }
        public bool IsAdmin { get; set; }

        public ClientsPage()
        {
            InitializeComponent();
            ClientsCollection = new ObservableCollection<Customer>();
            _allCustomers = new ObservableCollection<Customer>();
            DataGrid.ItemsSource = ClientsCollection;
            IsAdmin = GlobalClass.CurrentUser?.IsAdmin ?? false;
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            _allCustomers.Clear();
            ClientsCollection.Clear();
            var data = ProcedureDB.GetCustomers();
            foreach (var item in data)
            {
                _allCustomers.Add(item);
                ClientsCollection.Add(item);
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
            ClientsCollection.Clear();

            foreach (var customer in _allCustomers)
            {
                if (string.IsNullOrEmpty(searchText) ||
                    customer.FullName?.ToLower().Contains(searchText) == true ||
                    customer.Email?.ToLower().Contains(searchText) == true ||
                    customer.Phone?.ToLower().Contains(searchText) == true)
                {
                    ClientsCollection.Add(customer);
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
            GlobalClass.MainFrame?.Navigate(new AddEditCustomerPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                GlobalClass.MainFrame?.Navigate(new AddEditCustomerPage(SelectedItem));
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
                    ProcedureDB.DeleteCustomer(SelectedItem.CustomerId);
                    LoadData();
                }
            }
        }
    }
}
