using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PharmacyEvo.Models
{
    public class AuthModel : INotifyPropertyChanged
    {
        private string _loginOrEmail;
        private string _password;

        public string LoginOrEmail
        {
            get => _loginOrEmail;
            set
            {
                if (_loginOrEmail == value) return;
                _loginOrEmail = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


