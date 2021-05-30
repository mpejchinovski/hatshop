using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace hatshop.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime DateTime { get; set; }

        public ICollection<OrderHat> Hats { get; set; }

        public double Total
        {
            get { return Hats != null ? Hats.Sum(h => h.Hat.Price) : 0; }
        }
    }
}
