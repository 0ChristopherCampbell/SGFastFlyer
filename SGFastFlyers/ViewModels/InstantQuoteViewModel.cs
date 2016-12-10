//-----------------------------------------------------------------------
// <copyright file="InstantQuoteViewModel.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Utility;

    public class InstantQuoteViewModel
    {
        public int ID { get; set; }

        /* - If sending out emails to 'lock in' a quote without an order with expiry date (Phase 2)
        public string FirstName { get; set; }
        [Required, Display(Name = "Email Address")]
        public string EmailAddress { get; set; }
        */

        [Required, Range(5000, int.MaxValue, ErrorMessage = "Please enter a multiple of {1}"), Display(Name ="Quantity Needed?")]
        public int Quantity { get; set; }

        [Required, Display(Name = "Is this considered a metro area?")]
        public bool IsMetro { get; set; }

        [Required, Display(Name = "Do you require printing?")]
        public bool NeedsPrint { get; set; }

        [Required, Display(Name = "Pick a size for your leaflet.")]
        public Enums.PrintSize PrintSize { get; set; }

        [Required, Display(Name = "Do you require double sided printing?")]
        public Enums.PrintFormat PrintFormat
        {
            get
            {
                if (IsDoubleSided)
                    {
                    return Enums.PrintFormat.DoubleSided;
                }
                else
                {
                    return Enums.PrintFormat.Standard;
                }                
            }
        }

        public decimal? Cost
        {
            get
            {
                decimal cost = Config.CostPer1000() * 1000 ?? -1;
                if (this.IsMetro)
                {
                    return cost;
                }
                else
                {
                    return cost + Config.NonMetroAddition();
                }
            }
            set { }
        }

        [Display(Name = "Your Instant Quote:")]
        public string FormattedCost
        {
            get { return string.Format("{0:C}", Cost); }
        }

        public bool IsDoubleSided { get; set; }

        public static SelectList QuantityList
        {
            get { return new SelectList(dropDownQuantity); }
        }

        public static IEnumerable<int> dropDownQuantity = Enumerable.Range(1, 10).Select(x => x * 5000);
    }
  
}
