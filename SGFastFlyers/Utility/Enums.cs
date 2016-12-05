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
        A1 = 1,
        A2 = 2,
        A3 = 3,
        A4 = 4,
        A5 = 5
            
        /// Etc etc    
    }

    /// <summary>
    /// The format of the flyer, double sided, folded etc
    /// </summary>
    public enum PrintFormat
    {
        Standard = 100,
        DoubleSided = 200,
        TriFold = 300,
        SomeOtherFormat = 305
            //etc..
    }
}