using System;
using Avalonia.Controls;
using FitnessApp.ViewModels;
using FitnessApp.Views;
using FitnessApp.Models;

namespace FitnessApp.Views;

public partial class AdminMainWindow : Window
{
    public AdminMainWindow()
    {
        InitializeComponent();
    }

    public AdminMainWindow(Administrator admin) : this()
    {
        var viewModel = new AdminMainViewModel(admin);
        viewModel.CloseRequested += () =>
        {
            new LoginWindow().Show();
            this.Close();
        };
        DataContext = viewModel;
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is AdminMainViewModel viewModel)
        {
            viewModel.CloseRequested += () =>
            {
                new LoginWindow().Show();
                this.Close();
            };
        }
    }
}