using System.Windows;
using System.Windows.Controls;
using PharmacyEvo.Global;

namespace PharmacyEvo.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public bool IsCustomer { get; set; }
        public bool IsManeger { get; set; }
        public bool IsAdmin { get; set; }
        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
            IsCustomer = GlobalClass.CurrentUser.IsCustomer;
            IsAdmin = GlobalClass.CurrentUser.IsAdmin;
            IsManeger = GlobalClass.CurrentUser.IsManeger;
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            GlobalClass.MainFrame.Navigate(new ProfilePage());
        }
    }
}


