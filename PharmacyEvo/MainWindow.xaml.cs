using System.Windows;

namespace PharmacyEvo
{
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
