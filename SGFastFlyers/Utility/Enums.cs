//-----------------------------------------------------------------------
// <copyright file="Config.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// The paper size for an order
    /// </summary>
    public enum PrintSize
    {   
        DL = 1,
        A5 = 5            
        /// Etc etc    
    }

    /// <summary>
    /// The format of the flyer, double sided, folded etc
    /// </summary>
    public enum PrintFormat
    {
        Standard = 100,
        DoubleSided = 200
    }
}