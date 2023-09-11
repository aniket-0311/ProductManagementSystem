using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProductManagementSystem_Assign2.Models.ViewModel
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomValidationForPassword : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            if (password.Length < 6)
            {
                ErrorMessage = "Password must have at least 6 characters.";
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                ErrorMessage = "Password must have at least 1 uppercase letter.";
                return false;
            }

            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?\""\\:|<>{}]"))
            {
                ErrorMessage = "Password must have at least 1 special character.";
                return false;
            }

            return true;
        }
    }
}
