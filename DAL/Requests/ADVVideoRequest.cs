﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Requests
{
    public class ADVVideoRequest
    {
        /*
         Svojstva videosadržaja su name, description, image, total time, streaming url, genre i tags.
         */
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ImageId must be a positive number(integer)")]
        public int ImageId { get; set; }
        [Range(1, 2145600, ErrorMessage = "Length must be between 1 and 2145600 seconds")]
        public int TotalTime { get; set; }
        [Url(ErrorMessage = "Invalid URL format")]
        public string StreamingUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "GenreId must be a positive number(integer)")]
        public int GenreId { get; set; }
        public List<string> Tags { get; set; }
    }
}
