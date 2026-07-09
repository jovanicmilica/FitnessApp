using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessApp.Models;
using FitnessApp.Repositories;
using FitnessApp.Enums;
using FitnessApp.Views;
using System;

namespace FitnessApp.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    private readonly ClientRepository clientRepository;
    private readonly TrainerRepository trainerRepository;
    private readonly AdminRepository adminRepository;

    public LoginViewModel()
    {
        clientRepository = new ClientRepository();
        trainerRepository = new TrainerRepository();
        adminRepository = new AdminRepository();
    }

    [RelayCommand]
    private void Login()
    {
        ErrorMessage = string.Empty;

        // Validacija
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Please fill in all fields.";
            return;
        }

        // Provjeri Client
        Client client = clientRepository.GetByEmail(Email);
        if (client != null && client.Password == Password)
        {
            var window = new ClientMainWindow();
            window.DataContext = new ClientMainViewModel(client);
            window.Show();
            CloseRequested?.Invoke();
            return;
        }

        /*
        // Provjeri Trainer
        Trainer trainer = trainerRepository.GetByEmail(Email);
        if (trainer != null && trainer.Password == Password)
        {
            if (trainer.Status != TrainerStatus.ACTIVE)
            {
                ErrorMessage = "Your account is pending approval or has been suspended.";
                return;
            }
            var window = new TrainerMainWindow();
            window.DataContext = new TrainerMainViewModel(trainer);
            window.Show();
            CloseRequested?.Invoke();
            return;
        }
        */

        // Provjeri Administrator
        Administrator admin = adminRepository.GetByEmail(Email);
        if (admin != null && admin.Password == Password)
        {
            var window = new AdminMainWindow();
            window.DataContext = new AdminMainViewModel(admin);
            window.Show();
            CloseRequested?.Invoke();
            return;
        }

        // Ako nista nije pronadjeno
        ErrorMessage = "Invalid email or password.";
       
    }

    [RelayCommand]
    private void OpenRegisterClient()
    {
        var window = new RegisterClientWindow();
        window.DataContext = new RegisterClientViewModel();
        window.Show();
    }

    [RelayCommand]
    private void OpenRegisterTrainer()
    {
        var window = new RegisterTrainerWindow();
        window.DataContext = new RegisterTrainerViewModel();
        window.Show();
    }

    // Event za zatvaranje prozora iz ViewModela
    public event Action CloseRequested;
}