namespace FitnessApp.Models;

using System;
using System.Collections.Generic;

public abstract class User
{
    private int id;
    private string firstName;
    private string lastName;
    private string email;
    private string password;

    public int Id { get => id; set => id = value; }
    public string FirstName { get => firstName; set => firstName = value; }
    public string LastName { get => lastName; set => lastName = value; }
    public string Email { get => email; set => email = value; }
    public string Password { get => password; set => password = value; }

    public bool Login(string email, string password) { return false; }
    public void Logout() { }
}