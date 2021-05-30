using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using hatshop.Models;

namespace hatshop.ViewModels
{
    public class HatPictureViewModel
    {
        public Hat Hat { get; set; }

        [Display(Name = "Picture")]
        public IFormFile Picture { get; set; }
    }
}
