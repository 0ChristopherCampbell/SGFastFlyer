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
                decimal quantity = Quantity / 1000;
                decimal cost = 0;
                decimal dlSingleSided = 0;
                decimal dlDoubleSided = 0;
                decimal A5SingleSided = 0;
                decimal A5DoubleSided = 0;

                if(quantity >= 1 && quantity < 20)
                {
                    cost = 50;
                    dlSingleSided = 40;
                    dlDoubleSided = 43;
                    A5SingleSided = 52;
                    A5DoubleSided = 55.9m;
                }
                if (quantity >= 20 && quantity < 50)
                {
                    cost = 49;
                    dlSingleSided = 38;
                    dlDoubleSided = 41;
                    A5SingleSided = 49.4m;
                    A5DoubleSided = 53.3m;
                }
                if (quantity >= 50 && quantity < 75)
                {
                    cost = 48;
                    dlSingleSided = 36;
                    dlDoubleSided = 41;
                    A5SingleSided = 46.8m;
                    A5DoubleSided = 53.3m;
                }
                if (quantity >= 75 && quantity <= 100)
                {
                    cost = 47;
                    dlSingleSided = 36;
                    dlDoubleSided = 41;
                    A5SingleSided = 46.8m;
                    A5DoubleSided = 53.3m;
                }
                if (quantity >= 100 && quantity <= 200)
                {
                    cost = 46;
                    dlSingleSided = 34;
                    dlDoubleSided = 38;
                    A5SingleSided = 44.2m;
                    A5DoubleSided = 49.4m;
                }
                if (quantity >= 200 && quantity < 300)
                {
                    cost = 45;
                    dlSingleSided = 34;
                    dlDoubleSided = 38;
                    A5SingleSided = 44.2m;
                    A5DoubleSided = 49.4m;
                }
                if (quantity >= 300)
                {
                    cost = 42;
                    dlSingleSided = 32;
                    dlDoubleSided = 36;
                    A5SingleSided = 41.6m;
                    A5DoubleSided = 46.8m;
                }
                cost = cost * quantity;
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
                            cost = cost + dlDoubleSided*quantity;
                        }
                        if (PrintSize == Enums.PrintSize.A5)
                        {
                            cost = cost + A5DoubleSided * quantity;
                        }
                    }
                    else
                    {

                        if (PrintSize == Enums.PrintSize.DL)
                        {
                            cost = cost + dlSingleSided * quantity;
                        }

                        if (PrintSize == Enums.PrintSize.A5)
                        {
                            cost = cost + A5SingleSided * quantity;
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
       
