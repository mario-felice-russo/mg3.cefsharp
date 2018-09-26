using Models;
using MVVM;
using Repositories;
using System.Windows;

namespace mg3.contacts
{
    /// <summary>
    /// Interaction logic for NewContactWindow.xaml
    /// </summary>
    public partial class NewContactWindow : Window
    {
        public NewContactWindow()
        {
            InitializeComponent();
        }

        private ContactRepository repository = new ContactRepository();

        public RelayCommand OnSaveContact
        {
            get
            {
                return _onSaveContact ?? (
                    _onSaveContact = new RelayCommand(
                        async (p) =>
                        {
                            Contact contact = new Contact()
                            {
                                Name = nameTextBox.Text,
                                Email = emailTextBox.Text,
                                Phone = phoneNumberTextBox.Text
                            };

                            await repository.InsertAsync(contact);
                            Close();
                        }
                    )
                );
            }
        }
        private RelayCommand _onSaveContact = null;
    }
}
