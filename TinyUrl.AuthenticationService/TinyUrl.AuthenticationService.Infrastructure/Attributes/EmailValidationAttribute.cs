using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TinyUrl.AuthenticationService.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EmailValidationAttribute : ValidationAttribute
    {
        private static readonly Regex EmailRegex = new Regex(
      @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
      RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Check if the value is null or empty
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult("Email is required.");
            }

            // Check if the value is a valid email
            if (!EmailRegex.IsMatch(value.ToString()))
            {
                return new ValidationResult("Invalid email format.");
            }

            return ValidationResult.Success;
        }
    }
}
