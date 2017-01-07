//-----------------------------------------------------------------------
// <copyright file="Quote.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// A quote, given to the customer prior to ordering
    /// </summary>
    public class Quote
    {
        public int ID { get; set; }

        /// <summary>
        /// Once an order is made the quote will have reference to the order it was used in
        /// </summary>
        [ForeignKey("Order")]
        public int OrderID { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }
        
        [Required]
        public bool IsMetro {get; set; }
            [Required]
        public decimal Cost { get; set; }

      
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-Mmm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ExpiryDate
        {
            get { return DateTime.Now.AddMonths(1); }
        }
        
        public virtual Order Order { get; set; }
    }
}