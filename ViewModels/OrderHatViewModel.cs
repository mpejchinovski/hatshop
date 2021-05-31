using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using hatshop.Models;

namespace hatshop.ViewModels
{
    public class OrderHatViewModel
    {
        public int HatId { get; set; }
        public int Quantity { get; set; }
    }
}
