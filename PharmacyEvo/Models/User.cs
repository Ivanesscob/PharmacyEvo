using System;

namespace PharmacyEvo.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;

        public bool IsCustomer { get; set; }
    }
}


