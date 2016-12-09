//-----------------------------------------------------------------------
// <copyright file="ExampleController.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell (Some info taken from http://stackoverflow.com/questions/25343645/how-can-i-restrict-access-to-certain-views-and-actions-in-mvc-5) </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Standards.Classes
{
    //TODO: Make ExampleModel etc and wire it all up with a view
    using System.Web.Mvc;

    // In your controllers/actions you can use the Authorize attribute and specify a list of acceptable roles.
    // For example, the following would limit the entire controller to only those in an "Admin" role:
    [Authorize(Roles = "Admin")]
    [RequireHttps] // These can be line by line or inline seperated by commas
    public class ExampleController : Controller
    {
        private bool someVariable;

        // GET: Example
        // If there's a particular action in that controller that anyone should be able to access, logged in or not,
        // you can still protect the entire controller but use AllowAnonymous on the action in question:
        [AllowAnonymous]        
        public ActionResult Index()
        {
            return View();
        }

        // GET: Example/Details/5
        //You can also just add the Authorize attribute directly on your action(s):
        [Authorize(Roles = "User")]
        public ActionResult Details(int? id)
        {
            var exampleDetails = "data";
            return View(exampleDetails);
        }

        // GET: Example/Create
        public ActionResult Create()
        {
            return View();
        }
        
        /// <summary>
        /// POST: Example/PostEvent
        /// </summary>
        /// <returns>Some Post Event</returns>
        [HttpPost, ValidateAntiForgeryToken] // AntiForgeryToken to prevent overposting
        public ActionResult PostEvent(string someData)
        {
            return View();
        }        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //Dispose tasks here, db.Dispose etc..
            }
            base.Dispose(disposing);
        }
    }
}
