//-----------------------------------------------------------------------
// <copyright file="PolicyDocumentsController.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Adam Campbell </author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGFastFlyers.Controllers
{
    public class PolicyDocumentsController : Controller
    {
        // GET: PolicyDocuments
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TermsandConditions()//ToDo: all need to be changed to full screen instead of object.
        {
            return View();
}

        public ActionResult PrivacyPolicy()
        {
            return View();
        }
        public ActionResult ShippingandReturnsPolicy()
        {
            return View();
        }
        public ActionResult MetroAreas()
        {
            return View();
        }
    }
}
