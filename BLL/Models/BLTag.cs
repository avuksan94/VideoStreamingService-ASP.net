using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class BLTag
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<BLVideoTag> VideoTags { get; set; } = new List<BLVideoTag>();
    }
}
