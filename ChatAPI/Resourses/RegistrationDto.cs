using ChatAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Resourses
{
	public class RegistrationDto
	{
		[EmailUserUnique]
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime LastActive { get; set; }

		public RegistrationDto()
		{
			CreatedAt = DateTime.Now;
			LastActive = DateTime.Now;
		}
	}
}
