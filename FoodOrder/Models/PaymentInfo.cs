using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FoodWeb.Models
{
    [Table("PaymentInfo")]
    public class PaymentInfo
    {
        [Key]
        public int pay_id { get; set; }
        public int userId { get; set; }
        public string ch_tx_id { get; set; }
        public string card_type { get; set; }
        public string exp_date { get; set; }
        public string email_id { get; set; }
        public string tx_status { get; set; }
        public decimal amount { get; set; }
    }
}