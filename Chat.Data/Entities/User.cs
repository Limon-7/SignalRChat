using Chat.Data.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Chat.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Last name is Required")]
        [EmailAddress]
        [EmailUserUnique]
        public string Email { get; set; }
        [Required(ErrorMessage="First name is Required")]
        [MinLength(3,ErrorMessage = "First name should be atleast 2 Charecters")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is Required")]
        [MinLength(3, ErrorMessage = "First name should be atleast 2 Charecters")]
        public string LastName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }

        public ICollection<Message> MessageSent { get; set; }
        public ICollection<Message> MessageReceived { get; set; }
    }


    public class EmailUserUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var _context = (ChatContext)validationContext.GetService(typeof(ChatContext));
            var entity = _context.Users.SingleOrDefault(e => e.Email == value.ToString());

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string email)
        {
            return $"Email {email} is already in use";
        }
    }
}
