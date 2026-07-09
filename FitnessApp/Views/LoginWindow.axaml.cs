using Avalonia.Controls;
using FitnessApp.ViewModels;

namespace FitnessApp.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        var viewModel = new LoginViewModel();
        
        viewModel.CloseRequested += () => this.Close();
        
        DataContext = viewModel;
    }
}