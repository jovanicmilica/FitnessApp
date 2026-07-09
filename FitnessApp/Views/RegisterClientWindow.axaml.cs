using Avalonia.Controls;
using FitnessApp.ViewModels;

namespace FitnessApp.Views;

public partial class RegisterClientWindow : Window
{
    public RegisterClientWindow()
    {
        InitializeComponent();
        var viewModel = new RegisterClientViewModel();
        viewModel.CloseRequested += () => this.Close();
        DataContext = viewModel;
    }
}