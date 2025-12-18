using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditMedicineBatchPage.xaml
    /// </summary>
    public partial class AddEditMedicineBatchPage : Page
    {
        private MedicineBatch _medicineBatch;
        private bool _isEditMode;
        private ObservableCollection<Medicine> _medicines;
        private ObservableCollection<Supplier> _suppliers;

        public AddEditMedicineBatchPage(MedicineBatch medicineBatch = null)
        {
            InitializeComponent();
            _medicineBatch = medicineBatch;
            _isEditMode = medicineBatch != null;

            _medicines = new ObservableCollection<Medicine>(ProcedureDB.GetMedicines());
            MedicineComboBox.ItemsSource = _medicines;

            _suppliers = new ObservableCollection<Supplier>(ProcedureDB.GetSuppliers());
            SupplierComboBox.ItemsSource = _suppliers;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать партию лекарств";
                MedicineComboBox.SelectedValue = _medicineBatch.MedicineId;
                QuantityTextBox.Text = _medicineBatch.Quantity.ToString();
                ExpirationDatePicker.SelectedDate = _medicineBatch.ExpirationDate;
                SupplierComboBox.SelectedValue = _medicineBatch.SupplierId;
            }
            else
            {
                TitleTextBlock.Text = "Добавить партию лекарств";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new MedicineBatchesPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MedicineComboBox.SelectedValue == null ||
                string.IsNullOrWhiteSpace(QuantityTextBox.Text) ||
                !int.TryParse(QuantityTextBox.Text, out _) ||
                ExpirationDatePicker.SelectedDate == null ||
                SupplierComboBox.SelectedValue == null)
            {
                MessageBox.Show("Заполните все поля корректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var medicineBatch = new MedicineBatch
            {
                BatchId = _isEditMode ? _medicineBatch.BatchId : 0,
                MedicineId = (int)MedicineComboBox.SelectedValue,
                Quantity = int.Parse(QuantityTextBox.Text),
                ExpirationDate = ExpirationDatePicker.SelectedDate.Value,
                SupplierId = (int)SupplierComboBox.SelectedValue
            };

            ProcedureDB.UpsertMedicineBatch(medicineBatch);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
