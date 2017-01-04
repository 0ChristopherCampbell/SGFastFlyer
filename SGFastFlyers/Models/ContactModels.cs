//-----------------------------------------------------------------------
// <copyright file="ContactModels.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Adam Campbell </author>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SGFastFlyers.Models
{
    public class ContactModels
    {
        [Required(ErrorMessage ="First Name is required.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
    }
    public class EmailQuotes
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        
    }
}
