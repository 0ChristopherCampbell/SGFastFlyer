//-----------------------------------------------------------------------
// <copyright file="CreateOrderViewModel.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Utility;

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

        [ Display(Name = "Paper Size")]
        public Enums.PrintSize? PrintSize { get; set; }

        [Required, Display(Name = "Do you require double sided printing?")]
        public Enums.PrintFormat PrintFormat { get; set; }


        public decimal Cost
        {
            get
            {
                decimal cost = (decimal)Quantity / 1000 * Config.CostPer1000() ?? -1;


                  
                if (!IsMetro == false)
                {
                    cost = cost + (decimal)Config.NonMetroAddition();
                }



                if (NeedsPrint)


                {
                    if (IsDoubleSided)

                    {

                        if (PrintSize ==  Enums.PrintSize.DL)

                        {
                            cost = cost + (Quantity / 1000 * (decimal)Config.DLDoubleSidedPer1000());
                        }
                        if (PrintSize == Enums.PrintSize.A5)
                        {
                            cost = cost + (Quantity / 1000 * (decimal)Config.A5DoubleSidedPer1000());
                        }
                    }
                    else
                    {

                        if (PrintSize == Enums.PrintSize.DL)
                        {
                            cost = cost + (Quantity / 1000 * (decimal)Config.DLSingleSidedPer1000());
                        }

                        if (PrintSize ==  Enums.PrintSize.A5)
                        {
                            cost = cost + (Quantity / 1000 * (decimal)Config.A5SingleSidedPer1000());
                        }
                    }
                }
                if (cost < 400)
                {
                    cost = 400;
                }

 
                    return cost;
               
            }
            set { }
        }

        [Display(Name = "Total Quoted Cost:")]
        public string FormattedCost
        {
            get { return string.Format("{0:C}", Cost); }
        }
        public bool IsDoubleSided { get; set; }
    }
}