using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private LibraryContext _context;
        private ObservableCollection<Book> _books;
        private ObservableCollection<Author> _authors;
        private ObservableCollection<Genre> _genres;
        private string _searchText = string.Empty;
        private Author? _selectedAuthor;
        private Genre? _selectedGenre;
        private Book? _selectedBook;  // НОВОЕ: выбранная книга

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            _context = new LibraryContext();
            _books = new ObservableCollection<Book>();
            _authors = new ObservableCollection<Author>();
            _genres = new ObservableCollection<Genre>();
            LoadData();
        }

        public ObservableCollection<Book> Books
        {
            get => _books;
            set 
            { 
                _books = value; 
                OnPropertyChanged(nameof(Books)); 
            }
        }

        public ObservableCollection<Author> Authors
        {
            get => _authors;
            set 
            { 
                _authors = value; 
                OnPropertyChanged(nameof(Authors)); 
            }
        }

        public ObservableCollection<Genre> Genres
        {
            get => _genres;
            set 
            { 
                _genres = value; 
                OnPropertyChanged(nameof(Genres)); 
            }
        }

        public string SearchText
        {
            get => _searchText;
            set 
            { 
                _searchText = value; 
                OnPropertyChanged(nameof(SearchText)); 
                FilterBooks(); 
            }
        }

        public Author? SelectedAuthor
        {
            get => _selectedAuthor;
            set 
            { 
                _selectedAuthor = value; 
                OnPropertyChanged(nameof(SelectedAuthor)); 
                FilterBooks(); 
            }
        }

        public Genre? SelectedGenre
        {
            get => _selectedGenre;
            set 
            { 
                _selectedGenre = value; 
                OnPropertyChanged(nameof(SelectedGenre)); 
                FilterBooks(); 
            }
        }

        // НОВОЕ: свойство для выбранной книги
        public Book? SelectedBook
        {
            get => _selectedBook;
            set 
            { 
                _selectedBook = value; 
                OnPropertyChanged(nameof(SelectedBook));
            }
        }

        // ОБНОВЛЕННЫЕ КОМАНДЫ с проверкой
        public ICommand AddBookCommand => new RelayCommand(_ => AddBook());
        public ICommand EditBookCommand => new RelayCommand(_ => EditBook(), _ => SelectedBook != null);
        public ICommand DeleteBookCommand => new RelayCommand(_ => DeleteBook(), _ => SelectedBook != null);
        public ICommand ManageAuthorsCommand => new RelayCommand(_ => ManageAuthors());
        public ICommand ManageGenresCommand => new RelayCommand(_ => ManageGenres());

        private void LoadData()
        {
            try
            {
                _context.Books.Include(b => b.Author).Include(b => b.Genre).Load();
                _context.Authors.Load();
                _context.Genres.Load();

                Books = new ObservableCollection<Book>(_context.Books.Local);
                Authors = new ObservableCollection<Author>(_context.Authors.Local);
                Genres = new ObservableCollection<Genre>(_context.Genres.Local);
                
                System.Diagnostics.Debug.WriteLine($"Загружено книг: {Books.Count}");
                System.Diagnostics.Debug.WriteLine($"Загружено авторов: {Authors.Count}");
                System.Diagnostics.Debug.WriteLine($"Загружено жанров: {Genres.Count}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private void FilterBooks()
        {
            try
            {
                var query = _context.Books
                    .Include(b => b.Author)
                    .Include(b => b.Genre)
                    .AsQueryable();

                if (!string.IsNullOrWhiteSpace(SearchText))
                    query = query.Where(b => b.Title.Contains(SearchText));

                if (SelectedAuthor != null)
                    query = query.Where(b => b.AuthorId == SelectedAuthor.Id);

                if (SelectedGenre != null)
                    query = query.Where(b => b.GenreId == SelectedGenre.Id);

                Books = new ObservableCollection<Book>(query.ToList());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка фильтрации: {ex.Message}");
            }
        }

        private void AddBook()
        {
            try
            {
                var window = new Views.BookWindow(_context.Authors.ToList(), _context.Genres.ToList());
                window.Owner = Application.Current.Windows.OfType<Views.MainWindow>().FirstOrDefault();
                
                if (window.ShowDialog() == true && window.Book != null)
                {
                    _context.Books.Add(window.Book);
                    _context.SaveChanges();
                    FilterBooks();
                    
                    MessageBox.Show("Книга успешно добавлена!", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении книги: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // НОВЫЙ МЕТОД: редактирование книги
        private void EditBook()
        {
            try
            {
                // Проверяем, выбрана ли книга
                if (SelectedBook == null)
                {
                    MessageBox.Show("Выберите книгу для редактирования", "Внимание", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var editWindow = new Views.EditBookWindow(SelectedBook, _context.Authors.ToList(), _context.Genres.ToList());
                editWindow.Owner = Application.Current.Windows.OfType<Views.MainWindow>().FirstOrDefault();
                
                if (editWindow.ShowDialog() == true)
                {
                    // Сохраняем изменения
                    _context.Entry(SelectedBook).State = EntityState.Modified;
                    _context.SaveChanges();
                    
                    // Обновляем отображение
                    FilterBooks();
                    
                    MessageBox.Show("Книга успешно обновлена!", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании книги: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // НОВЫЙ МЕТОД: удаление книги
        private void DeleteBook()
        {
            try
            {
                // Проверяем, выбрана ли книга
                if (SelectedBook == null)
                {
                    MessageBox.Show("Выберите книгу для удаления", "Внимание", 
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var result = MessageBox.Show(
                    $"Удалить книгу '{SelectedBook.Title}'?\n" +
                    $"Автор: {SelectedBook.Author?.LastName}\n" +
                    $"Жанр: {SelectedBook.Genre?.Name}\n" +
                    $"В наличии: {SelectedBook.QuantityInStock}",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _context.Books.Remove(SelectedBook);
                    _context.SaveChanges();
                    
                    // Обновляем отображение
                    FilterBooks();
                    
                    MessageBox.Show("Книга успешно удалена!", "Успех", 
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении книги: {ex.Message}\n\nВнутренняя ошибка: {ex.InnerException?.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ManageAuthors()
        {
            try
            {
                var window = new Views.AuthorsWindow(_context);
                window.Owner = Application.Current.Windows.OfType<Views.MainWindow>().FirstOrDefault();
                window.ShowDialog();
                LoadData(); // Перезагружаем данные после закрытия окна
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ошибка при управлении авторами: {ex.Message}";
                
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nВнутренняя ошибка: {ex.InnerException.Message}";
                }
                
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ManageGenres()
        {
            try
            {
                var window = new Views.GenresWindow(_context);
                window.Owner = Application.Current.Windows.OfType<Views.MainWindow>().FirstOrDefault();
                window.ShowDialog();
                LoadData(); // Перезагружаем данные после закрытия окна
            }
            catch (Exception ex)
            {
                string errorMessage = $"Ошибка при управлении жанрами: {ex.Message}";
                
                if (ex.InnerException != null)
                {
                    errorMessage += $"\nВнутренняя ошибка: {ex.InnerException.Message}";
                }
                
                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}