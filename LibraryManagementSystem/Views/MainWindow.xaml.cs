using System.Windows;
using LibraryManagementSystem.ViewModels;

namespace LibraryManagementSystem.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.SearchText = string.Empty;
                viewModel.SelectedAuthor = null;
                viewModel.SelectedGenre = null;
            }
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is MainViewModel viewModel && viewModel.SelectedBook != null)
            {
                viewModel.EditBookCommand.Execute(null);
            }
        }
    }
}