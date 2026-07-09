using System;
using Avalonia.Controls;
using FitnessApp.Models;
using FitnessApp.ViewModels;
using FitnessApp.Views;

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
        
        if (DataContext is TrainerMainViewModel viewModel)
        {
            viewModel.CloseRequested += () =>
            {
                new LoginWindow().Show();
                this.Close();
            };
        }
    }
}