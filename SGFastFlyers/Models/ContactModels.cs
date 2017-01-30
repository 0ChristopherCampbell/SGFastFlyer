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

        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{8,15}$", ErrorMessage = "Please enter valid phone no. Mobile: 0411222333 or Landline: 0711112222. Thank you.")]
        public string PhoneNumber { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
    }
    public class EmailQuotes
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public int Quantity { get; set; }

        
    }
    public class DirectDebitEmail
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required."), EmailAddress]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public int Quantity { get; set; }


    }
}
