using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public ObservableCollection<Order> OrdersCollection { get; set; }
        private ObservableCollection<Order> _allOrders;
        public Order SelectedItem { get; set; }
        public bool IsAdmin { get; set; }

        public OrdersPage()
        {
            InitializeComponent();
            OrdersCollection = new ObservableCollection<Order>();
            _allOrders = new ObservableCollection<Order>();
            DataGrid.ItemsSource = OrdersCollection;
            IsAdmin = GlobalClass.CurrentUser?.IsAdmin ?? false;
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            _allOrders.Clear();
            OrdersCollection.Clear();
            var data = ProcedureDB.GetOrders();
            foreach (var item in data)
            {
                _allOrders.Add(item);
                OrdersCollection.Add(item);
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
            OrdersCollection.Clear();

            foreach (var order in _allOrders)
            {
                if (string.IsNullOrEmpty(searchText) ||
                    order.OrderDate.ToString("dd.MM.yyyy").Contains(searchText))
                {
                    OrdersCollection.Add(order);
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
            GlobalClass.MainFrame?.Navigate(new AddEditOrderPage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                GlobalClass.MainFrame?.Navigate(new AddEditOrderPage(SelectedItem));
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
                    ProcedureDB.DeleteOrder(SelectedItem.OrderId);
                    LoadData();
                }
            }
        }
    }
}
