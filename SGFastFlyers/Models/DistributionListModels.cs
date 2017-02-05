
using System.Collections.Generic;

namespace SGFastFlyers.Models
{
    public class DistributionListModels
    {
        public DistributionListModels()
        {
            DistributionList = new List<DistributionListModels>();
        }

        public string DeliveryArea { get; set; }
        public string Region { get; set; }
        public int TotalUnits { get; set; }
        public List<DistributionListModels> DistributionList { get; set; }
    }
}