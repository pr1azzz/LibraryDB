using System;
using System.Windows;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Views
{
    public partial class EditAuthorWindow : Window
    {
        public Author CurrentAuthor { get; private set; }

        public EditAuthorWindow(Author author)
        {
            InitializeComponent();
            
            CurrentAuthor = author;
            
            // Заполняем поля данными
            FirstNameBox.Text = author.FirstName;
            LastNameBox.Text = author.LastName;
            BirthDatePicker.SelectedDate = author.BirthDate.ToLocalTime();
            CountryBox.Text = author.Country;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка заполнения
                if (string.IsNullOrWhiteSpace(FirstNameBox.Text))
                {
                    MessageBox.Show("Введите имя автора", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                if (string.IsNullOrWhiteSpace(LastNameBox.Text))
                {
                    MessageBox.Show("Введите фамилию автора", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                
                if (BirthDatePicker.SelectedDate == null)
                {
                    MessageBox.Show("Выберите дату рождения", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Обновляем данные
                CurrentAuthor.FirstName = FirstNameBox.Text;
                CurrentAuthor.LastName = LastNameBox.Text;
                CurrentAuthor.BirthDate = BirthDatePicker.SelectedDate.Value.ToUniversalTime();
                CurrentAuthor.Country = CountryBox.Text;

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}