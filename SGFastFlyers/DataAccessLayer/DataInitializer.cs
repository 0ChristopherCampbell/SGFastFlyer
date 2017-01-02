using System;
using System.Collections.Generic;
using System.Data.Entity;
//-----------------------------------------------------------------------
// <copyright file="OrderInitializer.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.DataAccessLayer
{
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Models;

    public class DataInitializer : CreateDatabaseIfNotExists<SGDbContext>
    {
        string defaultAdminUserName = "admin@sgfastflyers.com.au";
        string defaultAdminPassword = "defaultpassword_1";

        protected override void Seed(SGDbContext context)
        {
            createRolesAndDefaultUser();

            var orders = new List<Order>
            {
            new Order{FirstName="Carson",LastName="Alexander",EmailAddress="ACarson@email.com", PhoneNumber="0400000000", Quantity=10000, IsPaid=false},
            new Order{FirstName="Meredith",LastName="Alonso",EmailAddress="AMeredith@email.com", PhoneNumber="0411111111", Quantity=34000, IsPaid=true},
            new Order{FirstName="Arturo",LastName="Anand",EmailAddress="AArturo@email.com", PhoneNumber="0422222222", Quantity=9800, IsPaid=true},
            new Order{FirstName="Gytis",LastName="Barzdukas",EmailAddress="BGytis@email.com", PhoneNumber="0433333333", Quantity=20000, IsPaid=false},
            new Order{FirstName="Yan",LastName="Li",EmailAddress="LYan@email.com", PhoneNumber="0444444444", Quantity=13000, IsPaid=false},
            new Order{FirstName="Peggy",LastName="Justice",EmailAddress="JPeggy@email.com", PhoneNumber="0455555555", Quantity=12000, IsPaid=true},
            new Order{FirstName="Laura",LastName="Norman",EmailAddress="NLaura@email.com", PhoneNumber="0466666666", Quantity=30900, IsPaid=false},
            new Order{FirstName="Nino",LastName="Olivetto",EmailAddress="ONino@email.com", PhoneNumber="0477777777", Quantity=10000, IsPaid=false}
            };

            orders.ForEach(o => context.Orders.Add(o));
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

            var printDetails = new List<PrintDetail>
            {
            new PrintDetail{OrderID=1, PrintFormat=Enums.PrintFormat.DoubleSided, NeedsPrint=true, PrintSize=Enums.PrintSize.DL},
            new PrintDetail{OrderID=2, PrintFormat=Enums.PrintFormat.DoubleSided, NeedsPrint=true, PrintSize=Enums.PrintSize.DL},
            new PrintDetail{OrderID=3, PrintFormat=Enums.PrintFormat.Standard, NeedsPrint=true, PrintSize=Enums.PrintSize.A5},
            new PrintDetail{OrderID=4, PrintFormat=Enums.PrintFormat.DoubleSided, NeedsPrint=false, PrintSize=Enums.PrintSize.A5},
            new PrintDetail{OrderID=5, PrintFormat=Enums.PrintFormat.Standard, NeedsPrint=true, PrintSize=Enums.PrintSize.A5},
            new PrintDetail{OrderID=6, PrintFormat=Enums.PrintFormat.Standard, NeedsPrint=true, PrintSize=Enums.PrintSize.A5},
            new PrintDetail{OrderID=7, NeedsPrint=false, PrintFormat = null, PrintSize = null},
            new PrintDetail{OrderID=8, NeedsPrint=false, PrintFormat = null, PrintSize = null}
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

            var quotes = new List<Quote>
            {
            new Quote{OrderID=1, Cost=99999, IsMetro = true, Quantity = 20000},
            new Quote{OrderID=2, Cost=99999, IsMetro = true, Quantity = 20000},
            new Quote{OrderID=3, Cost=99999, IsMetro = true, Quantity = 20000},
            new Quote{OrderID=4, Cost=99999, IsMetro = true, Quantity = 20000},
            new Quote{OrderID=5, Cost=99999, IsMetro = true, Quantity = 20000},
            new Quote{OrderID=6, Cost=99999, IsMetro = true, Quantity = 20000},
            new Quote{OrderID=7, Cost=99999, IsMetro = true, Quantity = 20000}
            };
            quotes.ForEach(q => context.Quotes.Add(q));
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

        public bool createRolesAndDefaultUser()
        {
            ApplicationDbContext appContext = new ApplicationDbContext();

            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(appContext));
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(appContext));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {
                // Create Admin role. 
                roleManager.Create(new IdentityRole("Admin"));
            }

            // Create default admin superuser.    
            ApplicationUser user = new ApplicationUser()
            {
                UserName = defaultAdminUserName
            };

            var chkUser = userManager.Create(user, defaultAdminPassword);

            //Add default User to Role Admin   
            if (chkUser.Succeeded)
            {
                IdentityResult result = userManager.AddToRole(user.Id, "Admin");
                return chkUser.Succeeded;
            }
            else
            {
                return chkUser.Succeeded;
            }            
        }
    }
}