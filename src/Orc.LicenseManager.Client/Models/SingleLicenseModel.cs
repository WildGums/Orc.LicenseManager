// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleLicenseModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Models
{
    using Catel.Data;

    /// <summary>
    /// This is the model for passing data to the SingleLicenseViewModel
    /// </summary>
    public class SingleLicenseModel : ModelBase
    {
        #region Properties
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the PurchaseLink.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public string PurchaseLink { get; set; }

        /// <summary>
        /// Gets or sets the company site.
        /// </summary>
        /// <value>
        /// The company site.
        /// </value>
        public string AboutSite { get; set; }

        /// <summary>
        /// Gets or sets the Title for the about box
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string AboutTitle { get; set; }

        /// <summary>
        /// Gets or sets the company text that will be used in the about box
        /// </summary>
        /// <value>
        /// The company text.
        /// </value>
        public string AboutText { get; set; }

        /// <summary>
        /// Gets or sets the AboutImage source path.
        /// </summary>
        /// <value>
        /// The image source.
        /// </value>
        public string AboutImage { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        #endregion
    }
}