﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Club.Models
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Creator { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}