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
        
        RefreshData();
    }
    
    private void RefreshData()
    {
        clientRepository.Reload();
        trainerRepository.Reload();
        adminRepository.Reload();
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
        
        RefreshData();

        Client loggedClient = clientRepository.GetByEmail(Email);
        if (loggedClient != null && loggedClient.Password == Password)
        {
            var window = new ClientMainWindow(loggedClient);
            window.Show();
            CloseRequested?.Invoke();
            return;
        }

        Trainer loggedTrainer = trainerRepository.GetByEmail(Email);
        if (loggedTrainer != null && loggedTrainer.Password == Password)
        {
            if (loggedTrainer.Status != TrainerStatus.ACTIVE)
            {
                ErrorMessage = "Your account is pending approval or has been suspended.";
                return;
            }
            var window = new TrainerMainWindow(loggedTrainer);
            window.Show();
            CloseRequested?.Invoke();
            return;
        }

        Administrator loggedAdmin = adminRepository.GetByEmail(Email);
        if (loggedAdmin != null && loggedAdmin.Password == Password)
        {
            var window = new AdminMainWindow(loggedAdmin);
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