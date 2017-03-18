namespace SGFastFlyers.Utility
{
    /// <summary>
    /// Used for pricing utilities such as calculating cost for an order.
    /// </summary>
    public static class PricingHelper
    {        
        /// <summary>
        /// Calculates the cost of an order
        /// </summary>
        /// <param name="quantity">the number of flyers</param>
        /// <param name="isMetro">whether this is a metro order</param>
        /// <param name="needsPrint">Whether this order needs printing</param>
        /// <param name="isDoubleSided">Whether this order is double sided</param>
        /// <param name="printSize">The print size of the order</param>
        /// <returns>The cost of the order</returns>
        public static decimal CalculateCost(int quantity, bool isMetro, bool needsPrint, bool isDoubleSided, Enums.PrintSize? printSize)
        {
            decimal decimalQuantity = decimal.Divide(quantity, 1000);
            decimal cost = 0;
            decimal dlSingleSided = 0;
            decimal dlDoubleSided = 0;
            decimal A5SingleSided = 0;
            decimal A5DoubleSided = 0;
            decimal gst = 0.1m;

            if (decimalQuantity >= 1 && decimalQuantity < 20)
            {
                cost = 48;
                dlSingleSided = 40;
                dlDoubleSided = 43;
                A5SingleSided = 52;
                A5DoubleSided = 55.9m;
            }

            if (decimalQuantity >= 20 && decimalQuantity < 50)
            {
                cost = 47;
                dlSingleSided = 38;
                dlDoubleSided = 41;
                A5SingleSided = 49.4m;
                A5DoubleSided = 53.3m;
            }

            if (decimalQuantity >= 50 && decimalQuantity < 75)
            {
                cost = 46;
                dlSingleSided = 36;
                dlDoubleSided = 41;
                A5SingleSided = 46.8m;
                A5DoubleSided = 53.3m;
            }

            if (decimalQuantity >= 75 && decimalQuantity <= 100)
            {
                cost = 45;
                dlSingleSided = 36;
                dlDoubleSided = 41;
                A5SingleSided = 46.8m;
                A5DoubleSided = 53.3m;
            }

            if (decimalQuantity >= 100 && decimalQuantity <= 200)
            {
                cost = 44;
                dlSingleSided = 34;
                dlDoubleSided = 38;
                A5SingleSided = 44.2m;
                A5DoubleSided = 49.4m;
            }

            if (decimalQuantity >= 200 && decimalQuantity < 300)
            {
                cost = 43;
                dlSingleSided = 34;
                dlDoubleSided = 38;
                A5SingleSided = 44.2m;
                A5DoubleSided = 49.4m;
            }

            if (decimalQuantity >= 300)
            {
                cost = 42;
                dlSingleSided = 32;
                dlDoubleSided = 36;
                A5SingleSided = 41.6m;
                A5DoubleSided = 46.8m;
            }

            cost = cost * decimalQuantity;
            if (cost < 364)
            {
                /// Charge minimum cost of 400
                return 400m;
            }

            if (!isMetro)
            {
                cost = cost + (decimal)Config.NonMetroAddition();
            }

            if (needsPrint)
            {
                if (isDoubleSided)
                {
                    if (printSize == Enums.PrintSize.DL)
                    {
                        cost = cost + dlDoubleSided * decimalQuantity;
                    }
                    else if (printSize == Enums.PrintSize.A5)
                    {
                        cost = cost + A5DoubleSided * decimalQuantity;
                    }
                }
                else
                {
                    if (printSize == Enums.PrintSize.DL)
                    {
                        cost = cost + dlSingleSided * decimalQuantity;
                    }
                    else if (printSize == Enums.PrintSize.A5)
                    {
                        cost = cost + A5SingleSided * decimalQuantity;
                    }
                }
            }

           

            //GST
            cost = decimal.Multiply(cost, gst) + cost;
            return cost;
        }
    }
}