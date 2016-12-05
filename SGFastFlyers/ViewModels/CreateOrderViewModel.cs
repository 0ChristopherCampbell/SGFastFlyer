using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SGFastFlyers.Models;
using System.ComponentModel.DataAnnotations;

namespace SGFastFlyers.ViewModels
{
    public class CreateOrderViewModel
    {
        public int ID { get; set; }
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required, Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        [Required, Display(Name = "Phone Number"), RegularExpression(@"^[0-9]*$")]
        public string PhoneNumber { get; set; }
        [Required]
        public int Quantity { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }
        [Display(Name = "Is this delivery classified as metro?")]
        public bool IsMetro { get; set; }
        public string DeliveryArea { get; set; }
        [Display(Name = "Do you require us to print your flyers?")]
        public bool NeedsPrint { get; set; }
        [Required, Display(Name = "Paper Size")]
        public Enums.PrintSize? PrintSize { get; set; }
        [Required, Display(Name = "Print Format")]
        public Enums.PrintFormat PrintFormat { get; set; }
        public decimal? Cost
        {
            // TODO: Does this need to be a property somewhere
            // TODO: Metro/Country Prices
            get { return (43 * (Quantity / 1000)); }
            set { value = (43 * (Quantity / 1000)); }
        }

        [Display(Name = "Total Quoted Cost:")]
        public string FormattedCost
        {
            get { return string.Format("{0:C}", Cost); }
        }
    }
}