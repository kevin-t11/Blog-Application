using System.ComponentModel.DataAnnotations;

public class ContactViewModel
{
    [Required(ErrorMessage = "Please enter your name.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Please enter your message.")]
    public string Message { get; set; }
}
