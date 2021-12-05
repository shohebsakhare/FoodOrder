﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodWeb.Models
{
    public class AdminLogin
    {
        [Key]
        public int adminid { get; set; }
        [StringLength(50)]
        [EmailAddress]
        [Required(ErrorMessage = "Email Required")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class UpdateLocation
    {
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int InvoiceId { get; set; }
    }
    public class Dashboard
    {
        public int Invoice { get; set; }
        public int Delivered { get; set; }
        public int Pending { get; set; }
        public int UserNo { get; set; }
    }
}