using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ChatAPI.Resourses
{
	public class MessageForCreationDto
	{
		public int SenderId { get; set; }
		public int RecipientId { get; set; }
		[Required]
		public string Content { get; set; }
		public DateTime MessageSent { get; set; }

		public MessageForCreationDto()
		{
			MessageSent = DateTime.Now;
		}
	}
}
