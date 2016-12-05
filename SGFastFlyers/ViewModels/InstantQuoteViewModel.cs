using System.ComponentModel.DataAnnotations;

namespace SGFastFlyers.ViewModels
{
    public class InstantQuoteViewModel
    {
        public int ID { get; set; }
        //public string FirstName { get; set; }
        //[Required, Display(Name = "Email Address")]
        //public string EmailAddress { get; set; }
        [Required, Range(5000, int.MaxValue)]
        public int Quantity { get; set; }
        [Required]
        public bool IsMetro { get; set; }
        [Required]
        public bool NeedsPrint { get; set; }
        [Required, Display(Name = "Pick a size for your leaflet")]
        public Enums.PrintSize PrintSize { get; set; }
        [Required, Display(Name = "Do you require double sided printing?")]
        public Enums.PrintFormat PrintFormat { get; set; }

        public decimal? Cost
        {
            // TODO: Does this need to be a property somewhere
            // TODO: Metro/Country Prices
            get { return (43 * (Quantity / 1000)); }
            set { value = (43 * (Quantity / 1000)); }
        }

        [Display(Name = "Your Instant Quote:")]
        public string FormattedCost
        {
            get { return string.Format("{0:C}", Cost); }
        }

    }
}