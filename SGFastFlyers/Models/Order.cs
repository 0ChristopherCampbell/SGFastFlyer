using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace SGFastFlyers.Models
{
    /// <summary>
    /// An order, defines personal information, printing details and delivery details
    /// </summary>
    public class Order
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
        [Required, Range(5000, int.MaxValue, ErrorMessage = "Please enter a multiple of {1}")]
        public int Quantity { get; set; }

        public virtual PrintDetail PrintDetail { get; set; }
        public virtual DeliveryDetail DeliveryDetail { get; set; }
        public virtual Quote Quote { get; set; }
    }
}