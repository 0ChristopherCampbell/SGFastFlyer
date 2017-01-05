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
    using System.Web;

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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Delivery Date")]
        public DateTime DeliveryDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is classified as a metro area.
        /// </summary>
        [Display(Name = "Is this delivery classified as metro?")]
 
        public bool IsMetro { get; set; }

        /// <summary>
        /// Gets or sets the delivery area
        /// </summary>
        [Display(Name = "Delivery Area")]
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

            set
            {
            }
        }
        /// <summary>
        /// Gets the attachment
        /// </summary>
       // [Required, Display(Name = "Please attach your file for printing.")]
       // public HttpPostedFileBase Attachment { get; set; }

        /// <summary>
        /// Gets or sets the cost of the new order.
        /// Price logic currently resides here.
        /// <para>
        /// TODO:
        ///     Work out a more centralized way to calculate cost. The formula below works for the time being
        ///     ....needs better input fields for updating.
        ///         -Won't have to update it across both views and controllers when the logic changes
        /// </para>
        /// </summary>
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