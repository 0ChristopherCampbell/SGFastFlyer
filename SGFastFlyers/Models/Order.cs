﻿//-----------------------------------------------------------------------
// <copyright file="Order.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// An order, defines personal information, printing details and delivery details
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        [Display(Name = "Order ID")]
        public int ID { get; set; }

        /// <summary>
        /// First Name of the customer
        /// </summary>
        [Required, Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last Name of the customer
        /// </summary>
        [Required, Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// Email Address of the customer
        /// </summary>
        [Required, Display(Name = "Email Address"), EmailAddress]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Phone Number of the customer
        /// </summary>
        [Required, Display(Name = "Phone Number"), RegularExpression(@"^[0-9]*$")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Quantity of Flyers/Brochures/etc being delivered
        /// </summary>
        [Required, Range(1, int.MaxValue, ErrorMessage = "Please enter a multiple of {1}")]
        public int Quantity { get; set; }

        /// <summary>
        /// Whether or not the order is paid, updated once payment is complete
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// Printing details:
        /// Is Printing Required? 
        ///     -Double Sided and Paper Size
        /// <see cref="PrintDetail"/> 
        /// </summary>
        public virtual PrintDetail PrintDetail { get; set; }

        /// <summary>
        /// Delivery details:
        ///     Address
        ///     TODO: Mapping (Phase 2)
        /// </summary>
        public virtual DeliveryDetail DeliveryDetail { get; set; }


        /// <summary>
        /// Attachment...only one at this stage.
        /// </summary>
       [Display(Name = "Attachment Details")]
        public virtual AttachmentDetail AttachmentDetail { get; set; }

        /// <summary>
        /// Quote details:
        ///     Stores data about the quote generated by the instant quote generator.
        ///     <see cref="Quote"/>
        /// </summary>
        public virtual Quote Quote { get; set; }
    }

}