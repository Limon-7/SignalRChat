﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Core.Resourses
{
    public class MessageForReturnDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderLastName { get; set; }
        public int RecipientId { get; set; }
        public string RecipientLastName { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}
