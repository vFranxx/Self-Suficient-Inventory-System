﻿namespace Shared.DTOs.SystemOperator
{
    public class AddOperatorDto
    {
        public required string Email { get; set; }
        public string? UserName { get; set; }
        public required string Password { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
