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
    using Models;
    using Utility;

    /// <summary>
    /// Create order view model, used on the order page to bind properties required for <see cref="Order"/> 
    /// <para>
    /// <seealso cref="PrintDetail"/>, <seealso cref="DeliveryDetail"/>, <seealso cref="Quote"/> 
    /// </para>
    /// </summary>
    public class CreateOrderViewModel
    {
        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [Required, Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [Required, Display(Name = "Phone Number"), RegularExpression(@"^[0-9]*$")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the delivery date
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is classified as a metro area.
        /// </summary>
        [Display(Name = "Is this delivery classified as metro?")]
        public bool IsMetro { get; set; }

        /// <summary>
        /// Gets or sets the delivery area
        /// </summary>
        public string DeliveryArea { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer requires printing.
        /// </summary>
        [Display(Name = "Do you require us to print your flyers?")]
        public bool NeedsPrint { get; set; }

        /// <summary>
        /// Gets or sets the size of the paper <see cref="Enums.PrintSize"/> for more details. 
        /// </summary>
        [Display(Name = "Paper Size")]
        public Enums.PrintSize? PrintSize { get; set; }

        /// <summary>
        /// Gets the printing format for the order <see cref="Enums.PrintFormat"/> for more details.
        /// </summary>
        [Required, Display(Name = "Do you require double sided printing?")]
        public Enums.PrintFormat PrintFormat
        {
            get
            {
                if (this.IsDoubleSided)
                {
                    return Enums.PrintFormat.DoubleSided;
                }
                else
                {
                    return Enums.PrintFormat.Standard;
                }
            }
        }

        /// <summary>
        /// Gets or sets the cost of the new order.
        /// Price logic currently resides here.
        /// <para>
        /// TODO:
        ///     Work out a more centralized way to calculate cost.
        ///         -Won't have to update it across both views and controllers when the logic changes
        /// </para>
        /// </summary>
        public decimal? Cost
        {
            get
            {
                decimal cost = (decimal)this.Quantity / 1000 * Config.BaseCostPer1000() ?? -1;
                  
                if (!this.IsMetro)
                {
                    cost = cost + (decimal)Config.NonMetroAddition();
                }

                if (this.NeedsPrint)
                {
                    if (this.IsDoubleSided)
                    {
                        if (this.PrintSize == Enums.PrintSize.DL)
                        {
                            cost = cost + ((decimal)this.Quantity / 1000 * (decimal)Config.DLDoubleSidedPer1000());
                        }

                        if (this.PrintSize == Enums.PrintSize.A5)
                        {
                            cost = cost + ((decimal)this.Quantity / 1000 * (decimal)Config.A5DoubleSidedPer1000());
                        }
                    }
                    else
                    {
                        if (this.PrintSize == Enums.PrintSize.DL)
                        {
                            cost = cost + ((decimal)this.Quantity / 1000 * (decimal)Config.DLSingleSidedPer1000());
                        }

                        if (this.PrintSize == Enums.PrintSize.A5)
                        {
                            cost = cost + ((decimal)this.Quantity / 1000 * (decimal)Config.A5SingleSidedPer1000());
                        }
                    }
                }

                if (cost < 400)
                {
                    cost = 400;
                } 

                return cost;               
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets the formatted cost (i.e. Dollars and cents (XXXXX.YY)
        /// </summary>
        [Display(Name = "Total Quoted Cost:")]
        public string FormattedCost
        {
            get { return string.Format("{0:C}", this.Cost); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the paper is double sided (to accommodate double sided checkboxes)
        /// </summary>
        public bool IsDoubleSided { get; set; }
    }
}