﻿using System.ComponentModel.DataAnnotations;

namespace Blazor_Markedsplads.Models
{
    public class LoginModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string? Password { get; set; }
    }
}