using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Views
{
    public partial class EditBookWindow : Window
    {
        public Book CurrentBook { get; private set; }
        private List<Author> _authors;
        private List<Genre> _genres;

        public EditBookWindow(Book book, List<Author> authors, List<Genre> genres)
        {
            InitializeComponent();
            
            CurrentBook = book;
            _authors = authors;
            _genres = genres;
            
            // Заполняем комбобоксы
            AuthorCombo.ItemsSource = _authors;
            GenreCombo.ItemsSource = _genres;
            
            // Заполняем поля данными
            TitleBox.Text = book.Title;
            AuthorCombo.SelectedItem = _authors.FirstOrDefault(a => a.Id == book.AuthorId);
            GenreCombo.SelectedItem = _genres.FirstOrDefault(g => g.Id == book.GenreId);
            YearBox.Text = book.PublishYear.ToString();
            IsbnBox.Text = book.ISBN;
            QuantityBox.Text = book.QuantityInStock.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверка заполнения
                if (string.IsNullOrWhiteSpace(TitleBox.Text))
                {
                    MessageBox.Show("Введите название книги", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (AuthorCombo.SelectedItem == null)
                {
                    MessageBox.Show("Выберите автора", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (GenreCombo.SelectedItem == null)
                {
                    MessageBox.Show("Выберите жанр", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(YearBox.Text, out int year) || year < 1000 || year > DateTime.Now.Year)
                {
                    MessageBox.Show($"Введите корректный год (1000-{DateTime.Now.Year})", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(QuantityBox.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Введите корректное количество (0 и более)", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Обновляем данные
                CurrentBook.Title = TitleBox.Text;
                CurrentBook.AuthorId = ((Author)AuthorCombo.SelectedItem).Id;
                CurrentBook.Author = (Author)AuthorCombo.SelectedItem;
                CurrentBook.GenreId = ((Genre)GenreCombo.SelectedItem).Id;
                CurrentBook.Genre = (Genre)GenreCombo.SelectedItem;
                CurrentBook.PublishYear = year;
                CurrentBook.ISBN = IsbnBox.Text;
                CurrentBook.QuantityInStock = quantity;

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