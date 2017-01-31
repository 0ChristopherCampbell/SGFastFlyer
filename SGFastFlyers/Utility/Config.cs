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
        public static decimal? NonMetroAddition()
        {
            return decimal.Parse(System.Configuration.ConfigurationManager.AppSettings["NonMetroAddition"]);
        }

        /// <summary>
        /// Web path to object data e.g /ObjectData
        /// </summary>
        public static string objectDataPath = "/ObjectData";

        public static string stripeKey = System.Configuration.ConfigurationManager.AppSettings["stripeKey"];
        public static string privateStripeKey = System.Configuration.ConfigurationManager.AppSettings["privateStripeKey"];
        public static string sgEmail = System.Configuration.ConfigurationManager.AppSettings["sgEmail"];

        #region CONSTANT VALUES
        public const string Yes = "Yes";
        public const string No = "No";
        #endregion
    }
}