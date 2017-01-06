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

        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a multiple of {1}"), Display(Name ="Quantity Needed?")]
        public int Quantity { get; set; }

        [Required, Display(Name = "Is this considered a metro area?")]
        public bool IsMetro { get; set; }

        [Required, Display(Name = "Do you require printing?")]
        public bool NeedsPrint { get; set; }

        [Required, Display(Name = "Pick a size for your leaflet.")]
        public Enums.PrintSize PrintSize { get; set; }

        [Required, Display(Name = "Do you require double sided printing?")]
        public Enums.PrintFormat PrintFormat { get; set; }


       

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
        public decimal Cost
        {
            get
            {
                decimal quantity = Quantity / 1000;
                decimal cost = 0;
                decimal dlSingleSided = 0;
                decimal dlDoubleSided = 0;
                decimal A5SingleSided = 0;
                decimal A5DoubleSided = 0;
                decimal gst = decimal.Divide(10, 100);
                if (quantity >= 1 && quantity < 20)
                {
                    cost = 50;
                    dlSingleSided = 40;
                    dlDoubleSided = 43;
                    A5SingleSided = 52;
                    A5DoubleSided = 55.9m;
                }
                if (quantity >= 20 && quantity < 50)
                {
                    cost = 49;
                    dlSingleSided = 38;
                    dlDoubleSided = 41;
                    A5SingleSided = 49.4m;
                    A5DoubleSided = 53.3m;
                }
                if (quantity >= 50 && quantity < 75)
                {
                    cost = 48;
                    dlSingleSided = 36;
                    dlDoubleSided = 41;
                    A5SingleSided = 46.8m;
                    A5DoubleSided = 53.3m;
                }
                if (quantity >= 75 && quantity <= 100)
                {
                    cost = 47;
                    dlSingleSided = 36;
                    dlDoubleSided = 41;
                    A5SingleSided = 46.8m;
                    A5DoubleSided = 53.3m;
                }
                if (quantity >= 100 && quantity <= 200)
                {
                    cost = 46;
                    dlSingleSided = 34;
                    dlDoubleSided = 38;
                    A5SingleSided = 44.2m;
                    A5DoubleSided = 49.4m;
                }
                if (quantity >= 200 && quantity < 300)
                {
                    cost = 45;
                    dlSingleSided = 34;
                    dlDoubleSided = 38;
                    A5SingleSided = 44.2m;
                    A5DoubleSided = 49.4m;
                }
                if (quantity >= 300)
                {
                    cost = 42;
                    dlSingleSided = 32;
                    dlDoubleSided = 36;
                    A5SingleSided = 41.6m;
                    A5DoubleSided = 46.8m;
                }
                cost = cost * quantity;
                if (IsMetro == false)
                {
                    cost = cost + (decimal)Config.NonMetroAddition();
                }



                if (NeedsPrint)


                {
                    if (IsDoubleSided)

                    {

                        if (PrintSize == Enums.PrintSize.DL)

                        {
                            cost = cost + dlDoubleSided * quantity;
                        }
                        if (PrintSize == Enums.PrintSize.A5)
                        {
                            cost = cost + A5DoubleSided * quantity;
                        }
                    }
                    else
                    {

                        if (PrintSize == Enums.PrintSize.DL)
                        {
                            cost = cost + dlSingleSided * quantity;
                        }

                        if (PrintSize == Enums.PrintSize.A5)
                        {
                            cost = cost + A5SingleSided * quantity;
                        }
                    }
                }
                if (cost < 364)
                {
                    cost = 364;
                }

                //GST
                cost = decimal.Multiply(cost, gst) + cost;
                return cost;

            }

            set
            {
            }
        }
    }
}
  

