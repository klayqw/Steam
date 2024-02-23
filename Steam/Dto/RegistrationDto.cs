using System.ComponentModel.DataAnnotations;

namespace Steam.Dto;

public class RegistrationDto
{
    [EmailAddress]
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}
