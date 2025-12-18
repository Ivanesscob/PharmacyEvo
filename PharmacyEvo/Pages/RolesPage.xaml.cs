using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Data;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class RolesPage : Page
    {
        public ObservableCollection<Role> RolesCollection { get; set; }
        private ObservableCollection<Role> _allRoles;
        public Role SelectedItem { get; set; }
        public bool IsAdmin { get; set; }

        public RolesPage()
        {
            InitializeComponent();
            RolesCollection = new ObservableCollection<Role>();
            _allRoles = new ObservableCollection<Role>();
            DataGrid.ItemsSource = RolesCollection;
            IsAdmin = GlobalClass.CurrentUser?.IsAdmin ?? false;
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            _allRoles.Clear();
            RolesCollection.Clear();
            var data = ProcedureDB.GetRoles();
            foreach (var item in data)
            {
                _allRoles.Add(item);
                RolesCollection.Add(item);
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
            RolesCollection.Clear();

            foreach (var role in _allRoles)
            {
                if (string.IsNullOrEmpty(searchText) ||
                    role.RoleName?.ToLower().Contains(searchText) == true)
                {
                    RolesCollection.Add(role);
                }
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
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
            GlobalClass.MainFrame?.Navigate(new AddEditRolePage());
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedItem != null)
            {
                GlobalClass.MainFrame?.Navigate(new AddEditRolePage(SelectedItem));
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
                    ProcedureDB.DeleteRole(SelectedItem.RoleId);
                    LoadData();
                }
            }
        }
    }
}
