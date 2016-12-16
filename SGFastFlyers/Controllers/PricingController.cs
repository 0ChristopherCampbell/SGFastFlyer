using System.Web.Mvc;
using SGFastFlyers.Utility;



namespace SGFastFlyers.Controllers
{
    public class PricingController : Controller
    {
        // GET: Pricing
        public ActionResult Index()
        {
            return View();
        }
        public int Quantity { get; set; }
        public bool IsMetro { get; set; }
        public bool NeedsPrint { get; set; }
        public Enums.PrintSize? PrintSize { get; set; }
        public Enums.PrintFormat PrintFormat { get; set; }
        public bool IsDoubleSided { get; private set; }

        public decimal? Cost
        {
            get
            {
                
                decimal cost = (decimal)Quantity / 1000 * Config.CostPer1000() ?? -1;
                decimal dlSingleSided = (Quantity / 1000 * (decimal)Config.DLSingleSidedPer1000());
                decimal dlDoubleSided = (Quantity / 1000 * (decimal)Config.DLDoubleSidedPer1000());
                decimal A5SingleSided = (Quantity / 1000 * (decimal)Config.A5SingleSidedPer1000());
                decimal A5DoubleSided = (Quantity / 1000 * (decimal)Config.DLSingleSidedPer1000());

                if(Quantity >= 1 && Quantity <= 19999)
                {
                    cost = 50;
                    dlSingleSided = 40;
                    dlDoubleSided = 43;
                    A5SingleSided = 52;
                    A5DoubleSided = 55.9m;
                }
                if (Quantity >= 20000 && Quantity <= 49999)
                {
                    cost = 49;
                    dlSingleSided = 38;
                    dlDoubleSided = 41;
                    A5SingleSided = 49.4m;
                    A5DoubleSided = 53.3m;
                }
                if (Quantity >= 50000 && Quantity <= 74999)
                {
                    cost = 48;
                    dlSingleSided = 36;
                    dlDoubleSided = 41;
                    A5SingleSided = 46.8m;
                    A5DoubleSided = 53.3m;
                }
                if (Quantity >= 75000 && Quantity <= 99999)
                {
                    cost = 47;
                    dlSingleSided = 36;
                    dlDoubleSided = 41;
                    A5SingleSided = 46.8m;
                    A5DoubleSided = 53.3m;
                }
                if (Quantity >= 100000 && Quantity <= 199999)
                {
                    cost = 46;
                    dlSingleSided = 34;
                    dlDoubleSided = 38;
                    A5SingleSided = 44.2m;
                    A5DoubleSided = 49.4m;
                }
                if (Quantity >= 200000 && Quantity <= 299999)
                {
                    cost = 45;
                    dlSingleSided = 34;
                    dlDoubleSided = 38;
                    A5SingleSided = 44.2m;
                    A5DoubleSided = 49.4m;
                }
                if (Quantity >= 1 && Quantity <= 19999)
                {
                    cost = 42;
                    dlSingleSided = 32;
                    dlDoubleSided = 36;
                    A5SingleSided = 41.6m;
                    A5DoubleSided = 46.8m;
                }

                if (!IsMetro == false)
                {
                    cost = cost + (decimal)Config.NonMetroAddition();
                }



                if (NeedsPrint)


                {
                    if (IsDoubleSided)

                    {

                        if (PrintSize == Enums.PrintSize.DL)

                        {
                            cost = cost + dlDoubleSided;
                        }
                        if (PrintSize == Enums.PrintSize.A5)
                        {
                            cost = cost + A5DoubleSided ;
                        }
                    }
                    else
                    {

                        if (PrintSize == Enums.PrintSize.DL)
                        {
                            cost = cost + dlSingleSided;
                        }

                        if (PrintSize == Enums.PrintSize.A5)
                        {
                            cost = cost + A5SingleSided;
                        }
                    }
                }
                if (cost < 400)
                {
                    cost = 400;
                }


                return cost;

            }
        }
    }
}
       
