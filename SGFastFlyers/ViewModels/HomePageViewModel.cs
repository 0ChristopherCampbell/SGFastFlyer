//-----------------------------------------------------------------------
// <copyright file="HomePageViewModel.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.ViewModels
{
    /// <summary>
    /// Houses all the models for the home page
    /// </summary>
    public class HomePageViewModel
    {
        /// <summary>
        /// Gets or sets an InstantQuoteViewModel that can then be accessed in the controller
        /// </summary>
        public InstantQuoteViewModel HomePageQuoteViewModel { get; set; }
    }
    
}