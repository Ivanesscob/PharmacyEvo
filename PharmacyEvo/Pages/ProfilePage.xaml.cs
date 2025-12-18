using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;
using PharmacyEvo.Windows;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            InitializeComponent();
            DataContext = GlobalClass.CurrentUser;
        }

        private void EditField_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.CurrentUser == null)
            {
                MessageBox.Show("Пользователь не найден. Сначала выполните вход.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var btn = sender as Button;
            if (btn == null)
                return;

            var field = btn.Tag as string;
            if (field == null)
                return;


            var user = GlobalClass.CurrentUser;
            var oldValue = GetFieldValue(user, field);

            var dlg = new EditValueWindow
            {
                Owner = Window.GetWindow(this),
                Caption = GetFieldCaption(field),
                Hint = GetFieldHint(field),
                Value = oldValue ?? ""
            };

            var ok = dlg.ShowDialog() == true;
            if (!ok) return;

            var newValue = (dlg.Value ?? "").Trim();
            if (string.IsNullOrWhiteSpace(newValue))
            {
                MessageBox.Show("Введите значение.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!TrySetFieldValue(user, field, newValue, out var error))
            {
                MessageBox.Show(error, "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            if (!ProcedureDB.UpdateUser(user))
            {
                
                TrySetFieldValue(user, field, oldValue ?? "", out _);
                MessageBox.Show("Не удалось обновить данные пользователя.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Данные обновлены.", "Успех",
                MessageBoxButton.OK, MessageBoxImage.Information);
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

        private static string GetFieldCaption(string field)
        {
            switch (field)
            {
                case "FullName":
                    return "Изменить ФИО";
                case "Email":
                    return "Изменить Email";
                case "Phone":
                    return "Изменить телефон";
                case "Password":
                    return "Изменить пароль";
                default:
                    return "Изменить";
            }
        }


        private static string GetFieldHint(string field)
        {
            switch (field)
            {
                case "Phone":
                    return "Введите телефон ";
                case "Password":
                    return "Введите новый пароль.";
                default:
                    return "Введите новое значение.";
            }
        }


        private static string GetFieldValue(User user, string field)
        {
            switch (field)
            {
                case "FullName":
                    return user.FullName;
                case "Email":
                    return user.Email;
                case "Phone":
                    return user.Phone;
                case "Password":
                    return user.Password;
                default:
                    return "";
            }
        }


        private static bool TrySetFieldValue(User user, string field, string value, out string error)
        {
            error = null;

            switch (field)
            {
                case "FullName":
                    user.FullName = value;
                    return true;
                case "Email":
                    user.Email = value;
                    return true;
                case "Phone":
                    user.Phone = value;
                    return true;
                case "Password":
                    user.Password = value;
                    return true;
                default:
                    error = "Неизвестное поле для редактирования.";
                    return false;
            }
        }
    }
}


