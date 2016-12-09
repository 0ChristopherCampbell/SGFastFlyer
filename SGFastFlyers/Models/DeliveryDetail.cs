//-----------------------------------------------------------------------
// <copyright file="DeliveryDetail.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DeliveryDetail
    {
        public int ID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        [Display(Name = "Delivery Date"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]    
        public DateTime DeliveryDate { get; set; }

        [Display(Name = "Delivery Area")]
        public string DeliveryArea { get; set; } // TODO: Work out data structure for delivery areas

        public virtual Order Order { get; set; }
    }
}