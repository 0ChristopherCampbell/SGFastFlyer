using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGFastFlyers.Utility
{
    public class Config
    {
        public static decimal? CostPer1000()
        {

            return decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["CostPer1000"]);           
           
        }

        public static decimal? NonMetroAddition()
        {
            return decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["NonMetroAddition"]);           

        }


    }
}