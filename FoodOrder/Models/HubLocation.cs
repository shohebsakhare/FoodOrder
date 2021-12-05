using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoodWeb.Models
{
    [Table("HubLocation")]
    public class HubLocation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Locations { get; set; }
        [Required]
        public string Address { get; set; }
    }
}