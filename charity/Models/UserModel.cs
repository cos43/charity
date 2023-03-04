using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace charity.Models;

public class UserModel
{
    [Key] public int Id { get; set; }

    [Required(ErrorMessage = "Please enter your first name.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Please enter your last name.")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Please enter your email address.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [UniqueEmail(ErrorMessage = "Email address is already registered.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Please enter your password.")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }


    [Required(ErrorMessage = "Please enter your phone number.")]
    [DataType(DataType.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Please enter your address.")]
    public string? Address { get; set; }

    [Required(ErrorMessage = "Please select a role.")]
    public UserRole? Role { get; set; }

    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var email = (string)value;
            var user = dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }


    public static string ComputeMD5Hash(string input)
    {
        using (var md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}

public enum UserRole
{
    Admin,
    NeedyPeople,
    Sponsor,
    Volunteer
}