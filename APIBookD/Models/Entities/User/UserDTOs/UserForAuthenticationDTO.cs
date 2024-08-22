﻿using System.ComponentModel.DataAnnotations;

namespace APIBookD.Models.Entities.User.UserDTOs
{
    public class UserForAuthenticationDTO
    {

        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
