// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleLicenseModel.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Models
{
    using Catel.Data;

    /// <summary>
    /// The license info model.
    /// </summary>
    public class LicenseInfo : ModelBase
    {
        public LicenseInfo()
        {
        }

        #region Properties
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the PurchaseUrl.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public string PurchaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the company site.
        /// </summary>
        /// <value>
        /// The company site.
        /// </value>
        public string InfoUrl { get; set; }

        /// <summary>
        /// Gets or sets the company text that will be used in the about box
        /// </summary>
        /// <value>
        /// The company text.
        /// </value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the ImageUri source path.
        /// </summary>
        /// <value>
        /// The image source.
        /// </value>
        public string ImageUri { get; set; }

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