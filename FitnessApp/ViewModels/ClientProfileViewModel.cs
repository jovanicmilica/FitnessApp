using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessApp.Models;
using FitnessApp.Repositories;
using System;
using System.Threading.Tasks;  

namespace FitnessApp.ViewModels
{
    public partial class ClientProfileViewModel : ObservableObject
    {
        private readonly Client _client;
        private readonly ClientRepository _clientRepository;

        [ObservableProperty]
        private string _firstName = string.Empty;

        [ObservableProperty]
        private string _lastName = string.Empty;

        [ObservableProperty]
        private string _email = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _saveMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isSaveSuccess;

        [ObservableProperty]
        private bool _isEditing;

        public ClientProfileViewModel(Client client)
        {
            _client = client;
            _clientRepository = new ClientRepository();
            LoadProfile();
        }

        private void LoadProfile()
        {
            FirstName = _client.FirstName ?? string.Empty;
            LastName = _client.LastName ?? string.Empty;
            Email = _client.Email ?? string.Empty;
            Password = _client.Password ?? string.Empty;
        }

        [RelayCommand]
        private void ToggleEdit()
        {
            IsEditing = !IsEditing;
            SaveMessage = string.Empty;
            IsSaveSuccess = false;
            
            if (!IsEditing)
            {
                LoadProfile();
            }
        }

        [RelayCommand]  
        private async Task SaveProfile()  
        {
            IsLoading = true;
            SaveMessage = string.Empty;
            IsSaveSuccess = false;

            try
            {
                // Validacija
                if (string.IsNullOrWhiteSpace(FirstName))
                {
                    SaveMessage = "First name is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(LastName))
                {
                    SaveMessage = "Last name is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Email))
                {
                    SaveMessage = "Email is required.";
                    return;
                }

                if (string.IsNullOrWhiteSpace(Password) || Password.Length < 6)
                {
                    SaveMessage = "Password must be at least 6 characters.";
                    return;
                }

                // Proveri da li email već postoji
                var existingClient = _clientRepository.GetByEmail(Email);
                if (existingClient != null && existingClient.Id != _client.Id)
                {
                    SaveMessage = "Email already in use by another account.";
                    return;
                }

                // Ažuriraj podatke
                _client.FirstName = FirstName;
                _client.LastName = LastName;
                _client.Email = Email;
                _client.Password = Password;

                _clientRepository.Update(_client);

                SaveMessage = "Profile updated successfully!";
                IsSaveSuccess = true;
                IsEditing = false;

                await Task.Delay(3000);
                SaveMessage = string.Empty;
            }
            catch (Exception ex)
            {
                SaveMessage = $"Error: {ex.Message}";
                IsSaveSuccess = false;
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}