//-----------------------------------------------------------------------
// <copyright file="Config.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Enums
{
    /// <summary>
    /// The paper size for an order
    /// </summary>
    public enum PrintSize
    {   
        /// <summary>
        /// DL paper size
        /// </summary>
        DL = 1,

        /// <summary>
        /// A5 paper size
        /// </summary>
        A5 = 5            
        /// Etc etc    
    }

    /// <summary>
    /// The format of the flyer, double sided, folded etc
    /// </summary>
    public enum PrintFormat
    {
        /// <summary>
        /// Single sided leaflet
        /// </summary>
        Standard = 100,

        /// <summary>
        /// Double sided leaflet
        /// </summary>
        DoubleSided = 200
    }
}