using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FaceApiWebMVC.Controllers
{
    public class OrderViewModel
    {
        [Display(Name = "Order ID")]
        public Guid OrderId { get; set; }

        [Display(Name = "Email")]
        public string UserEmail { get; set; }

        [Display(Name = "Image File")]
        public IFormFile File { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Order Status")]
        public string StatusString { get; set; }
        public byte[] ImageData { get; set; }
    }
}