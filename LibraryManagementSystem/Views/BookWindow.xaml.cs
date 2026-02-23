using System.Windows;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Views
{
    public partial class BookWindow : Window
    {
        public Book Book { get; private set; }

        public BookWindow(List<Author> authors, List<Genre> genres)
        {
            InitializeComponent();
            
            AuthorCombo.ItemsSource = authors;
            GenreCombo.ItemsSource = genres;
            
            Book = new Book();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TitleBox.Text))
            {
                MessageBox.Show("Введите название книги");
                return;
            }

            if (AuthorCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите автора");
                return;
            }

            if (GenreCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите жанр");
                return;
            }

            Book.Title = TitleBox.Text;
            Book.Author = (Author)AuthorCombo.SelectedItem;
            Book.AuthorId = Book.Author.Id;
            Book.Genre = (Genre)GenreCombo.SelectedItem;
            Book.GenreId = Book.Genre.Id;
            Book.PublishYear = int.Parse(YearBox.Text);
            Book.ISBN = IsbnBox.Text;
            Book.QuantityInStock = int.Parse(QuantityBox.Text);

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}