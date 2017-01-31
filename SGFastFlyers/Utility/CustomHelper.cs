using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SGFastFlyers.Utility
{
    public static class CustomHelper
    {        
        public static decimal CalcCost(int quant, bool isMetro, bool needsPrint, bool isDoubleSided, Enums.PrintSize? printSize)
        {
            decimal quantity = decimal.Divide(quant, 1000);
            decimal cost = 0;
            decimal dlSingleSided = 0;
            decimal dlDoubleSided = 0;
            decimal A5SingleSided = 0;
            decimal A5DoubleSided = 0;
            decimal gst = decimal.Divide(10, 100);

            if (quantity >= 1 && quantity < 20)
            {
                cost = 55;
                dlSingleSided = 40;
                dlDoubleSided = 43;
                A5SingleSided = 52;
                A5DoubleSided = 55.9m;
            }
            if (quantity >= 20 && quantity < 50)
            {
                cost = 54;
                dlSingleSided = 38;
                dlDoubleSided = 41;
                A5SingleSided = 49.4m;
                A5DoubleSided = 53.3m;
            }
            if (quantity >= 50 && quantity < 75)
            {
                cost = 53;
                dlSingleSided = 36;
                dlDoubleSided = 41;
                A5SingleSided = 46.8m;
                A5DoubleSided = 53.3m;
            }
            if (quantity >= 75 && quantity <= 100)
            {
                cost = 52;
                dlSingleSided = 36;
                dlDoubleSided = 41;
                A5SingleSided = 46.8m;
                A5DoubleSided = 53.3m;
            }
            if (quantity >= 100 && quantity <= 200)
            {
                cost = 51;
                dlSingleSided = 34;
                dlDoubleSided = 38;
                A5SingleSided = 44.2m;
                A5DoubleSided = 49.4m;
            }
            if (quantity >= 200 && quantity < 300)
            {
                cost = 50;
                dlSingleSided = 34;
                dlDoubleSided = 38;
                A5SingleSided = 44.2m;
                A5DoubleSided = 49.4m;
            }
            if (quantity >= 300)
            {
                cost = 48;
                dlSingleSided = 32;
                dlDoubleSided = 36;
                A5SingleSided = 41.6m;
                A5DoubleSided = 46.8m;
            }
            cost = cost * quantity;

            if (isMetro == false)
            {
                cost = cost + (decimal)Config.NonMetroAddition();
            }

            if (needsPrint)
            {
                if (isDoubleSided)
                {
                    if (printSize == Enums.PrintSize.DL)
                    {
                        cost = cost + dlDoubleSided * quantity;
                    }
                    if (printSize == Enums.PrintSize.A5)
                    {
                        cost = cost + A5DoubleSided * quantity;
                    }
                }
                else
                {
                    if (printSize == Enums.PrintSize.DL)
                    {
                        cost = cost + dlSingleSided * quantity;
                    }
                    if (printSize == Enums.PrintSize.A5)
                    {
                        cost = cost + A5SingleSided * quantity;
                    }
                }
            }
            if (cost < 364)
            {
                cost = 363.63636364m;
            }

            //GST
            cost = decimal.Multiply(cost, gst) + cost;
            return cost;
        }
    }
}