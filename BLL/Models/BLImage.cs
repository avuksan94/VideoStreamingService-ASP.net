﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BLImage
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public virtual ICollection<BLVideo> Videos { get; set; } = new List<BLVideo>();
    }
}
