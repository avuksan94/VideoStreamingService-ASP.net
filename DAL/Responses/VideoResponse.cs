using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Responses
{
    //Podaci koje zelimo da korisnici mogu vidjeti,nema potrebe prikazivati sve podatke
    public class VideoResponse
    {

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string? Image { get; set; }
        [Required]
        public int TotalTime { get; set; }
        [Required]
        public string StreamingUrl { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Tags { get; set; }
    }
}
