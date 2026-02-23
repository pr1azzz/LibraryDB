using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Views
{
    public partial class GenresWindow : Window
    {
        private LibraryContext _context;

        public GenresWindow(LibraryContext context)
        {
            InitializeComponent();
            _context = context;
            LoadGenres();
        }

        private void LoadGenres()
        {
            _context.Genres.Load();
            GenresGrid.ItemsSource = _context.Genres.Local.ToObservableCollection();
        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var genre = new Genre 
                { 
                    Name = "Новый жанр",
                    Description = "Описание"
                };
                
                _context.Genres.Add(genre);
                _context.SaveChanges();
                
                LoadGenres();
                MessageBox.Show("Жанр успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ошибка при добавлении жанра: {ex.Message}";
                
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nВнутренняя ошибка: {ex.InnerException.Message}";
                }
                
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void EditGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is Genre selectedGenre)
            {
                try
                {
                    var editWindow = new EditGenreWindow(selectedGenre);
                    editWindow.Owner = this;
                    
                    if (editWindow.ShowDialog() == true)
                    {
                        // Сохраняем изменения
                        _context.Entry(selectedGenre).State = EntityState.Modified;
                        _context.SaveChanges();
                        
                        // Обновляем отображение
                        LoadGenres();
                        
                        MessageBox.Show("Жанр успешно обновлен!", "Успех", 
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
                MessageBox.Show("Выберите жанр для редактирования", "Внимание", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresGrid.SelectedItem is Genre selectedGenre)
            {
                var result = MessageBox.Show(
                    $"Удалить жанр {selectedGenre.Name}?",
                    "Подтверждение",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _context.Genres.Remove(selectedGenre);
                        _context.SaveChanges();
                        LoadGenres();
                        MessageBox.Show("Жанр удален!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void GenresGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            EditGenre_Click(sender, e);
        }
    }
}