using ChatAPI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Models
{
	public class User
	{
		public int Id { get; set; }

        [Required]
        [EmailAddress]
        [EmailUserUnique]
		public string Email { get; set; }
		public string FirstName{get;set;}
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
