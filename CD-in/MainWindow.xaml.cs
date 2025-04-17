using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace CD_in;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
    {
        // Allow only digits
        e.Handled = !Regex.IsMatch(e.Text, @"^[0-9]+$");
    }
}