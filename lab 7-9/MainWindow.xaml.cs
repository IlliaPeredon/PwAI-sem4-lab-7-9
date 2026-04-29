using Contacts.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Contacts
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Contact> Contacts { get; set; }
        public ObservableCollection<Contact> ContactsView { get; set; }

        private Contact selectedContact;
        public Contact SelectedContact
        {
            get => selectedContact;
            set
            {
                selectedContact = value;
                ErrorMessage = "";
                DataContext = null;
                DataContext = this;
            }
        }

        public string ErrorMessage { get; set; }
        public string AggregationResult { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Contacts = new ObservableCollection<Contact>()
            {
                new Contact { Name="Alice", Age=25, Email="a@test.com", City="NY", Telephone="123" },
                new Contact { Name="Bob", Age=17, Email="b@test.com", City="LA", Telephone="456" }
            };

            ContactsView = new ObservableCollection<Contact>(Contacts);

            // Ensure SelectedContact is initialized so input TextBox bindings target an actual object.
            SelectedContact = new Contact();

            DataContext = this;
        }

        // ---------------- CRUD ----------------

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInputs(out string error))
            {
                ErrorMessage = error;
                Refresh();
                return;
            }

            if (Contacts.Any(c =>
                c.Name == SelectedContact.Name &&
                c.Email == SelectedContact.Email &&
                c.Telephone == SelectedContact.Telephone))
            {
                ErrorMessage = "Contact already exists!";
                Refresh();
                return;
            }

            Contacts.Add(new Contact
            {
                Name = SelectedContact.Name,
                Age = SelectedContact.Age,
                Email = SelectedContact.Email,
                City = SelectedContact.City,
                Telephone = SelectedContact.Telephone
            });

            ShowAll();
            ClearSelection();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            // After update → unbind (clear selection)
            ClearSelection();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedContact != null)
            {
                Contacts.Remove(SelectedContact);
                ShowAll();
                ClearSelection();
            }
        }

        // ---------------- VALIDATION ----------------

        private bool ValidateInputs(out string error)
        {
            error = "";

            if (SelectedContact == null ||
                string.IsNullOrWhiteSpace(SelectedContact.Name) ||
                string.IsNullOrWhiteSpace(SelectedContact.Email) ||
                string.IsNullOrWhiteSpace(SelectedContact.City) ||
                string.IsNullOrWhiteSpace(SelectedContact.Telephone))
            {
                error = "All fields must be filled!";
                return false;
            }

            return true;
        }

        // ---------------- LINQ ----------------

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            ContactsView = new ObservableCollection<Contact>(
                Contacts.Where(c => c.Age > 18));
            Refresh();
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            ContactsView = new ObservableCollection<Contact>(
                Contacts.OrderBy(c => c.Name));
            Refresh();
        }

        private void Project_Click(object sender, RoutedEventArgs e)
        {
            ContactsView = new ObservableCollection<Contact>(
                Contacts.Select(c => new Contact { Name = c.Name }));
            Refresh();
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            if (Contacts.Any(c => c.Age > 50))
            {
                ContactsView = new ObservableCollection<Contact>(
                    Contacts.Where(c => c.Age > 50));
            }
            else
            {
                ContactsView.Clear();
            }
            Refresh();
        }

        private void Aggregate_Click(object sender, RoutedEventArgs e)
        {
            double avg = Contacts.Average(c => c.Age);

            AggregationResult = $"Average Age: {avg:F2}";

            Refresh();
        }

        private void ShowAll_Click(object sender, RoutedEventArgs e)
        {
            ShowAll();
        }

        private void ShowAll()
        {
            ContactsView = new ObservableCollection<Contact>(Contacts);
            Refresh();
        }

        // ---------------- HELPERS ----------------

        private void ClearSelection()
        {
            SelectedContact = new Contact();
            Refresh();
        }

        private void Refresh()
        {
            DataContext = null;
            DataContext = this;
        }
    }
}