using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessApp.Models;
using FitnessApp.Repositories;

namespace FitnessApp.ViewModels;

public partial class RegisterClientViewModel : ObservableObject
{
    [ObservableProperty]
    private string firstName = string.Empty;

    [ObservableProperty]
    private string lastName = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    [ObservableProperty]
    private string successMessage = string.Empty;

    private readonly ClientRepository clientRepository;

    public RegisterClientViewModel()
    {
        clientRepository = new ClientRepository();
    }

    [RelayCommand]
    private void Register()
    {
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;

        // Validacija
        if (string.IsNullOrEmpty(FirstName) ||
            string.IsNullOrEmpty(LastName) ||
            string.IsNullOrEmpty(Email) ||
            string.IsNullOrEmpty(Password) ||
            string.IsNullOrEmpty(ConfirmPassword))
        {
            ErrorMessage = "Please fill in all fields.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        // Provjeri da li email vec postoji
        Client existing = clientRepository.GetByEmail(Email);
        if (existing != null)
        {
            ErrorMessage = "An account with this email already exists.";
            return;
        }

        // Kreiraj novog klijenta
        Client newClient = new Client
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password
        };

        clientRepository.Add(newClient);

        SuccessMessage = "Account created successfully! You can now log in.";

        // Ocisti formu
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
    }

    [RelayCommand]
    private void BackToLogin()
    {
        CloseRequested?.Invoke();
    }

    public event Action? CloseRequested;
}