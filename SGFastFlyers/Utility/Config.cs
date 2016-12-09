//-----------------------------------------------------------------------
// <copyright file="Config.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Glenn Campbell </author>
//-----------------------------------------------------------------------
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

        /// <summary>
        /// Used to make payments with eWay - Api Key
        /// </summary>
        public static string apiPaymentKey = "A1001C0zSw3ZlTUkh/KpkVAt6eBZzi7DgEDP3kpaV6k0fnWJGHxGBM6rGJ/cVe62W5aEV7"; //Chris Sandbox Key

        /// <summary>
        /// Used to make payments with eWay - Password
        /// </summary>
        public static string apiPaymentPassword = "kuUySEak";

        /// <summary>
        /// Defines whether or not payments made will go to the sandbox or to the real payment system
        /// </summary>
        public static string apiRapidEndpoint = "Sandbox"; // Use "Production" when live..
    }
}