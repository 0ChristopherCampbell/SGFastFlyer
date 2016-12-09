//-----------------------------------------------------------------------
// <copyright file="PrintDetail.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PrintDetail
    {
        public int ID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        [Required, Display(Name = "Printing Required?")]
        public bool NeedsPrint { get; set; }

        [Required, Display(Name = "Paper Size")]
        public Enums.PrintSize? PrintSize { get; set; }

        [Required, Display(Name = "Double Sided")]
        public Enums.PrintFormat? PrintFormat { get; set; }

        public virtual Order Order { get; set; }
        // Future: Print Design
    }
}