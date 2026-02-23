using System;
using System.Windows;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Views
{
    public partial class EditGenreWindow : Window
    {
        public Genre CurrentGenre { get; private set; }

        public EditGenreWindow(Genre genre)
        {
            InitializeComponent();
            
            CurrentGenre = genre;
            
            // Заполняем поля данными
            NameBox.Text = genre.Name;
            DescriptionBox.Text = genre.Description;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка заполнения
                if (string.IsNullOrWhiteSpace(NameBox.Text))
                {
                    MessageBox.Show("Введите название жанра", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Обновляем данные
                CurrentGenre.Name = NameBox.Text;
                CurrentGenre.Description = DescriptionBox.Text;

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