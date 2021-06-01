using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace hatshop.Models
{
    public class Hat
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public int? CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }

        [Display(Name = "Price (USD)")]
        public double Price { get; set; }

        public int Stock { get; set; }
        
        [JsonIgnore]
        public string PicturePath { get; set; }
        
        [JsonIgnore]
        public ICollection<OrderHat> Orders { get; set; }
    }
}
