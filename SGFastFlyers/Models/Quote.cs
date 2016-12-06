using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SGFastFlyers.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SGFastFlyers.Models
{
    /// <summary>
    /// A quote, given to the customer prior to ordering
    /// </summary>
    public class Quote
    {
        public int ID { get; set; }
        [Required, Range(5000, int.MaxValue)]
        public int Quantity { get; set; }
        
        [Required]
        public bool IsMetro { get; set; }

        [Required]
        public decimal? Cost
        {
            // TODO: Does this need to be a property somewhere
            // TODO: Metro/Country Prices
            get
            {
                if (IsMetro)
                {
                    return (decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["CostPer1000"]) * (Quantity / 1000)  );
                }
                else
                {
                    return (decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["CostPer1000"]) * (Quantity / 1000) + decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["NonMetroAddition"]));
                }
            }
            set { }         
        }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpiryDate
        {
            get { return DateTime.Now.AddMonths(1); }
        }

        /// <summary>
        /// Once an order is made the quote will have reference to the order it was used in
        /// </summary>
        [ForeignKey("Order")]
        public int? OrderID { get; set; }

        public virtual Order Order { get; set; }
    }
}