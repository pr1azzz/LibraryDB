using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Views
{
    public partial class AuthorsWindow : Window
    {
        private LibraryContext _context;

        public AuthorsWindow(LibraryContext context)
        {
            InitializeComponent();
            _context = context;
            LoadAuthors();
        }

        private void LoadAuthors()
        {
            _context.Authors.Load();
            AuthorsGrid.ItemsSource = _context.Authors.Local.ToObservableCollection();
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var author = new Author 
                { 
                    FirstName = "Новый", 
                    LastName = "Автор",
                    BirthDate = DateTime.UtcNow,
                    Country = "Россия"
                };
                
                _context.Authors.Add(author);
                _context.SaveChanges();
                
                LoadAuthors();
                MessageBox.Show("Автор успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ошибка при добавлении автора: {ex.Message}";
                
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nВнутренняя ошибка: {ex.InnerException.Message}";
                }
                
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ОБНОВЛЕННЫЙ МЕТОД РЕДАКТИРОВАНИЯ
        private void EditAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is Author selectedAuthor)
            {
                try
                {
                    var editWindow = new EditAuthorWindow(selectedAuthor);
                    editWindow.Owner = this;
                    
                    if (editWindow.ShowDialog() == true)
                    {
                        // Сохраняем изменения
                        _context.Entry(selectedAuthor).State = EntityState.Modified;
                        _context.SaveChanges();
                        
                        // Обновляем отображение
                        LoadAuthors();
                        
                        MessageBox.Show("Автор успешно обновлен!", "Успех", 
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при редактировании: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Выберите автора для редактирования", "Внимание", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsGrid.SelectedItem is Author selectedAuthor)
            {
                var result = MessageBox.Show(
                    $"Удалить автора {selectedAuthor.FirstName} {selectedAuthor.LastName}?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _context.Authors.Remove(selectedAuthor);
                        _context.SaveChanges();
                        LoadAuthors();
                        MessageBox.Show("Автор удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                    }
                }
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Добавляем двойной клик для быстрого редактирования
        private void AuthorsGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditAuthor_Click(sender, e);
        }
    }
}