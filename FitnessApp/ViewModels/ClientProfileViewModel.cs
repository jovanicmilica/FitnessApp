using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FitnessApp.Models;
using FitnessApp.Repositories;
using System;
using System.Threading.Tasks;  
using System.Collections.ObjectModel;
using System.Linq; 
using System.Collections.Generic;

namespace FitnessApp.ViewModels
{
    public partial class ClientProfileViewModel : ObservableObject
    {
        private readonly Client _client;
        private readonly ClientRepository _clientRepository;
        private readonly HealthRecordRepository _healthRecordRepository;
        private HealthRecord? _healthRecord;

        // Osnovni podaci
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
        
        // Health record
        [ObservableProperty]
        private ObservableCollection<string> _healthConditions = new();

        [ObservableProperty]
        private ObservableCollection<string> _disabilities = new();

        [ObservableProperty]
        private string _newHealthCondition = string.Empty;

        [ObservableProperty]
        private string _newDisability = string.Empty;

        public ClientProfileViewModel(Client client)
        {
            _client = client;
            _clientRepository = new ClientRepository();
            _healthRecordRepository = new HealthRecordRepository();
            
            LoadProfile();
            LoadHealthRecord();  
        }

        private void LoadProfile()
        {
            FirstName = _client.FirstName ?? string.Empty;
            LastName = _client.LastName ?? string.Empty;
            Email = _client.Email ?? string.Empty;
            Password = _client.Password ?? string.Empty;
        }
        
        private void LoadHealthRecord()
        {
            // Proveri da li klijent ima HealthRecordId
            if (_client.HealthRecordId == 0)
            {
                // Nema HealthRecord, kreiraj prazan
                _healthRecord = null;
                HealthConditions.Clear();
                Disabilities.Clear();
                return;
            }
            
            _healthRecord = _healthRecordRepository.GetById(_client.HealthRecordId);
    
            if (_healthRecord != null)
            {
                HealthConditions.Clear();
                Disabilities.Clear();
        
                foreach (var condition in _healthRecord.HealthConditions ?? new List<string>())
                {
                    HealthConditions.Add(condition);
                }
        
                foreach (var disability in _healthRecord.Disabilities ?? new List<string>())
                {
                    Disabilities.Add(disability);
                }
            }
            else
            {
                // HealthRecord sa tim ID-jem ne postoji
                HealthConditions.Clear();
                Disabilities.Clear();
            }
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
                LoadHealthRecord();  
                NewHealthCondition = string.Empty;
                NewDisability = string.Empty;
            }
        }
        
        [RelayCommand]
        private void AddHealthCondition()
        {
            if (string.IsNullOrWhiteSpace(NewHealthCondition))
                return;
    
            if (!HealthConditions.Contains(NewHealthCondition))
            {
                HealthConditions.Add(NewHealthCondition);
                NewHealthCondition = string.Empty;
            }
        }

        [RelayCommand]
        private void RemoveHealthCondition(string condition)
        {
            HealthConditions.Remove(condition);
        }

        [RelayCommand]
        private void AddDisability()
        {
            if (string.IsNullOrWhiteSpace(NewDisability))
                return;
    
            if (!Disabilities.Contains(NewDisability))
            {
                Disabilities.Add(NewDisability);
                NewDisability = string.Empty;
            }
        }

        [RelayCommand]
        private void RemoveDisability(string disability)
        {
            Disabilities.Remove(disability);
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
                
                // Ažuriraj HealthRecord
                if (_healthRecord != null)
                {
                    _healthRecord.HealthConditions = HealthConditions.ToList();
                    _healthRecord.Disabilities = Disabilities.ToList();
                    _healthRecordRepository.Update(_healthRecord);
                }
                else
                {
                    // Ako nema HealthRecord, kreiraj novi
                    var newRecord = new HealthRecord
                    {
                        HealthConditions = HealthConditions.ToList(),
                        Disabilities = Disabilities.ToList()
                    };
                    _healthRecordRepository.Add(newRecord);
                    _client.HealthRecordId = newRecord.Id;
                    _clientRepository.Update(_client);
                    _healthRecord = newRecord;
                }

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