using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Contacts.Models
{
    public class Contact : INotifyPropertyChanged
    {
        private string name;
        private int age;
        private string email;
        private string city;
        private string telephone;

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        public int Age
        {
            get => age;
            set { age = value; OnPropertyChanged(nameof(Age)); }
        }

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string City
        {
            get => city;
            set { city = value; OnPropertyChanged(nameof(City)); }
        }

        public string Telephone
        {
            get => telephone;
            set { telephone = value; OnPropertyChanged(nameof(Telephone)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}