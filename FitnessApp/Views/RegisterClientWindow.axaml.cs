using System;
using Avalonia.Controls;
using FitnessApp.ViewModels;

namespace FitnessApp.Views;

public partial class RegisterClientWindow : Window
{
    public RegisterClientWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is RegisterClientViewModel viewModel)
        {
            viewModel.CloseRequested += () => this.Close();
        }
    }
}