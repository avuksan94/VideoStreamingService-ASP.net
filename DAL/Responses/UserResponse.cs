﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? SecurityToken { get; set; }
    }
}
