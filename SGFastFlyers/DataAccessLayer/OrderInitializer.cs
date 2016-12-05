using System;
using System.Collections.Generic;
using System.Data.Entity;

using SGFastFlyers.Models;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace SGFastFlyers.DataAccessLayer
{
    public class OrderInitializer : DropCreateDatabaseIfModelChanges<OrderContext>
    {
        protected override void Seed(OrderContext context)
        {
            var orders = new List<Order>
            {
            new Order{FirstName="Carson",LastName="Alexander",EmailAddress="ACarson@email.com", PhoneNumber="0400000000", Quantity=10},
            new Order{FirstName="Meredith",LastName="Alonso",EmailAddress="AMeredith@email.com", PhoneNumber="0411111111", Quantity=34},
            new Order{FirstName="Arturo",LastName="Anand",EmailAddress="AArturo@email.com", PhoneNumber="0422222222", Quantity=980},
            new Order{FirstName="Gytis",LastName="Barzdukas",EmailAddress="BGytis@email.com", PhoneNumber="0433333333", Quantity=200},
            new Order{FirstName="Yan",LastName="Li",EmailAddress="LYan@email.com", PhoneNumber="0444444444", Quantity=13000},
            new Order{FirstName="Peggy",LastName="Justice",EmailAddress="JPeggy@email.com", PhoneNumber="0455555555", Quantity=1200},
            new Order{FirstName="Laura",LastName="Norman",EmailAddress="NLaura@email.com", PhoneNumber="0466666666", Quantity=3090},
            new Order{FirstName="Nino",LastName="Olivetto",EmailAddress="ONino@email.com", PhoneNumber="0477777777", Quantity=10000 }
            };

            orders.ForEach(o => context.Orders.Add(o));
            context.SaveChanges();
            var printDetails = new List<PrintDetail>
            {
            new PrintDetail{OrderID=1, PrintFormat=Enums.PrintFormat.DoubleSided, NeedsPrint=true, PrintSize=Enums.PrintSize.A1},
            new PrintDetail{OrderID=2, PrintFormat=Enums.PrintFormat.DoubleSided, NeedsPrint=true, PrintSize=Enums.PrintSize.A2},
            new PrintDetail{OrderID=3, PrintFormat=Enums.PrintFormat.Standard, NeedsPrint=true, PrintSize=Enums.PrintSize.A2},
            new PrintDetail{OrderID=4, PrintFormat=Enums.PrintFormat.DoubleSided, NeedsPrint=true, PrintSize=Enums.PrintSize.A5},
            new PrintDetail{OrderID=5, PrintFormat=Enums.PrintFormat.Standard, NeedsPrint=true, PrintSize=Enums.PrintSize.A5},
            new PrintDetail{OrderID=6, PrintFormat=Enums.PrintFormat.Standard, NeedsPrint=true, PrintSize=Enums.PrintSize.A5},
            new PrintDetail{OrderID=7, NeedsPrint=false},
            new PrintDetail{OrderID=8, NeedsPrint=false}
            };
            printDetails.ForEach(p => context.PrintDetails.Add(p));
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
            
            var deliveryDetails = new List<DeliveryDetail>
            {
            new DeliveryDetail{OrderID=1, DeliveryArea="SomeArea1", DeliveryDate=DateTime.Parse("2017/1/1")},
            new DeliveryDetail{OrderID=2, DeliveryArea="SomeArea2", DeliveryDate=DateTime.Parse("2017/1/14")},
            new DeliveryDetail{OrderID=3, DeliveryArea="SomeArea3", DeliveryDate=DateTime.Parse("2016/12/12")},
            new DeliveryDetail{OrderID=4, DeliveryArea="SomeArea4", DeliveryDate=DateTime.Parse("2016/12/17")},
            new DeliveryDetail{OrderID=5, DeliveryArea="SomeArea5", DeliveryDate=DateTime.Parse("2016/12/10")},
            new DeliveryDetail{OrderID=6, DeliveryArea="SomeArea6", DeliveryDate=DateTime.Parse("2017/1/16")},
            new DeliveryDetail{OrderID=7, DeliveryArea="SomeArea7", DeliveryDate=DateTime.Parse("2017/2/13")}
            };
            deliveryDetails.ForEach(d => context.DeliveryDetails.Add(d));
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }
        }
    }
}