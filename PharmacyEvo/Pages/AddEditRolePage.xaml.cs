using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class AddEditRolePage : Page
    {
        private Role _role;
        private bool _isEditMode;

        public AddEditRolePage(Role role = null)
        {
            InitializeComponent();
            _role = role;
            _isEditMode = role != null;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать роль";
                RoleNameTextBox.Text = _role.RoleName;
            }
            else
            {
                TitleTextBlock.Text = "Добавить роль";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new RolesPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(RoleNameTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var role = new Role
            {
                RoleId = _isEditMode ? _role.RoleId : 0,
                RoleName = RoleNameTextBox.Text
            };

            ProcedureDB.UpsertRole(role);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
