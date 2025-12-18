using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class AddEditSupplyPage : Page
    {
        private Supply _supply;
        private bool _isEditMode;
        private ObservableCollection<Supplier> _suppliers;

        public AddEditSupplyPage(Supply supply = null)
        {
            InitializeComponent();
            _supply = supply;
            _isEditMode = supply != null;

            _suppliers = new ObservableCollection<Supplier>(ProcedureDB.GetSuppliers());
            SupplierComboBox.ItemsSource = _suppliers;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать поставку";
                SupplierComboBox.SelectedValue = _supply.SupplierId;
                SupplyDatePicker.SelectedDate = _supply.SupplyDate;
            }
            else
            {
                TitleTextBlock.Text = "Добавить поставку";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new SuppliesPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (SupplierComboBox.SelectedValue == null || SupplyDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var supply = new Supply
            {
                SupplyId = _isEditMode ? _supply.SupplyId : 0,
                SupplierId = (int)SupplierComboBox.SelectedValue,
                SupplyDate = SupplyDatePicker.SelectedDate.Value
            };

            ProcedureDB.UpsertSupply(supply);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
