﻿using System.ComponentModel.DataAnnotations;

namespace MagicVillaApI2.Models
{
    public class Villa
    {
        
        public int Id { get; set; }
        [Required]  
        public string Name { get; set; }
        public string Details { get; set; }
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedDate { get; set; }
        public virtual IEnumerable<VillaNumber>? VillaNumbers { get; set; }=new List<VillaNumber>();
    }
}
