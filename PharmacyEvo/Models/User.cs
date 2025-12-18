using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PharmacyEvo.Models
{
    public class User : INotifyPropertyChanged
    {
        private int _userId;
        private string _fullName = string.Empty;
        private string _email = string.Empty;
        private string _phone = string.Empty;
        private string _password = string.Empty;
        private int _roleId;
        private DateTime _dateCreated = DateTime.Now;
        private bool _isActive = true;
        private bool _isCustomer;
        private bool _isManeger;
        private bool _isAdmin;

        public int UserId
        {
            get => _userId;
            set => SetField(ref _userId, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetField(ref _fullName, value);
        }

        public string Email
        {
            get => _email;
            set => SetField(ref _email, value);
        }

        public string Phone
        {
            get => _phone;
            set => SetField(ref _phone, value);
        }

        public string Password
        {
            get => _password;
            set => SetField(ref _password, value);
        }

        public int RoleId
        {
            get => _roleId;
            set => SetField(ref _roleId, value);
        }

        public DateTime DateCreated
        {
            get => _dateCreated;
            set => SetField(ref _dateCreated, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetField(ref _isActive, value);
        }

        public bool IsCustomer
        {
            get => _isCustomer;
            set => SetField(ref _isCustomer, value);
        }

        public bool IsManeger
        {
            get => _isManeger;
            set => SetField(ref _isManeger, value);
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetField(ref _isAdmin, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}


