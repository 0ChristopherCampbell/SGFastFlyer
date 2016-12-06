using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGFastFlyers.Enums
{
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