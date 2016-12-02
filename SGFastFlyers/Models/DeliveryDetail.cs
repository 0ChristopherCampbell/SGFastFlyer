using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGFastFlyers.Models
{
    public class DeliveryDetail
    {
        public int ID { get; set; }
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]    
        public DateTime DeliveryDate { get; set; }
        public string DeliveryArea { get; set; } // TODO: Work out data structure for delivery areas

        public virtual Order Order { get; set; }
    }
}