using System.ComponentModel.DataAnnotations;

namespace Blazor_Markedsplads.Models
{
    public class Customer
    {
        [Required, StringLength(50)] public string Name { get; set; }
        [Required, EmailAddress] public string Email { get; set; }
        [Range(18, 120)] public int Age { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public bool IsSeller { get; set; }
        [Required, MinLength(6)] public string Password { get; set; }
    }
}