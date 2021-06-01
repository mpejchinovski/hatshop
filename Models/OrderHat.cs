using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace hatshop.Models
{
    public class OrderHat
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        [JsonIgnore]
        public Order Order { get; set; }

        public int HatId { get; set; }
        public Hat Hat { get; set; }

        public int Quantity { get; set; }
    }
}
