using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BLVideoTag
    {
        public int Id { get; set; }

        public int VideoId { get; set; }

        public int TagId { get; set; }

        public virtual BLTag Tag { get; set; } = null!;

        public virtual BLVideo Video { get; set; } = null!;
        public override string ToString()
        {
            return $"Tag: {TagId}";
        }
    }
}
