using System;
using Avalonia.Controls;
using FitnessApp.ViewModels;

namespace FitnessApp.Views;

public partial class RegisterTrainerWindow : Window
{
    public RegisterTrainerWindow()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is RegisterTrainerViewModel viewModel)
        {
            viewModel.CloseRequested += () => this.Close();
        }
    }
}