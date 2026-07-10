using System;
using Avalonia.Controls;
using FitnessApp.Models;
using FitnessApp.ViewModels;
using FitnessApp.Views;

namespace FitnessApp.Views;

public partial class ClientMainWindow : Window
{
    public ClientMainWindow()
    {
        InitializeComponent();
    }

    public ClientMainWindow(Client client) : this()
    {
        var viewModel = new ClientMainViewModel(client);
        DataContext = viewModel;

        viewModel.LogoutRequested += () =>
        {
            new LoginWindow().Show();
            this.Close();
        };
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        
        if (DataContext is ClientMainViewModel viewModel)
        {
            viewModel.CloseRequested += () =>
            {
                new LoginWindow().Show();
                this.Close();
            };
        }
    }
}