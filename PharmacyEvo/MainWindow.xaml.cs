using System.Windows;

namespace PharmacyEvo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Global.GlobalClass.MainFrame.Navigate(new Pages.Auth.LoginPage());
            DataContext = new Global.GlobalClass();
        }
    }
}
