﻿using System.ComponentModel.DataAnnotations;

public class RegisterVM
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    public string Username { get; set; }
    [Required, DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string RepeatPassword { get; set; }
}