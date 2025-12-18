using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;
using PharmacyEvo.Models;

namespace PharmacyEvo.Pages
{
    public partial class AddEditCategoryPage : Page
    {
        private Category _category;
        private bool _isEditMode;

        public AddEditCategoryPage(Category category = null)
        {
            InitializeComponent();
            _category = category;
            _isEditMode = category != null;

            if (_isEditMode)
            {
                TitleTextBlock.Text = "Редактировать категорию";
                CategoryNameTextBox.Text = _category.CategoryName;
            }
            else
            {
                TitleTextBlock.Text = "Добавить категорию";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalClass.MainFrame != null && GlobalClass.MainFrame.CanGoBack)
            {
                GlobalClass.MainFrame.GoBack();
                return;
            }

            GlobalClass.MainFrame?.Navigate(new CategoriesPage());
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var category = new Category
            {
                CategoryId = _isEditMode ? _category.CategoryId : 0,
                CategoryName = CategoryNameTextBox.Text
            };

            ProcedureDB.UpsertCategory(category);
            BackButton_Click(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            BackButton_Click(sender, e);
        }
    }
}
