using System;
using Avalonia.Controls;
using FitnessApp.Models;
using FitnessApp.ViewModels;

namespace FitnessApp.Views;

public partial class TrainerMainWindow : Window
{
    public TrainerMainWindow()
    {
        InitializeComponent();
    }

    public TrainerMainWindow(Trainer trainer) : this()
    {
        var viewModel = new TrainerMainViewModel(trainer);
        DataContext = viewModel;

        viewModel.LogoutRequested += () =>
        {
            new LoginWindow().Show();
            this.Close();
        };
    }
}