﻿using UserService.Enums;

namespace UserService.Dto
{
    public class UserRegistrationRequest
    {
        public UserRole UserRole { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
    }
}
