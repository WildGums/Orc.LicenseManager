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
        public string CompanySite { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the company text that will be used in the singlelisencedialogue.
        /// </summary>
        /// <value>
        /// The company text.
        /// </value>
        public string CompanyText { get; set; }

        /// <summary>
        /// Gets or sets the CompanyImage source path.
        /// </summary>
        /// <value>
        /// The image source.
        /// </value>
        public string CompanyImage { get; set; }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets a value indicating whether [company website is set].
        /// </summary>
        /// <value>
        /// <c>true</c> if [company website is set]; otherwise, <c>false</c>.
        /// </value>
        public bool CompanyWebsiteIsSet
        {
            get { return CompanySite != null; }
        }

        /// <summary>
        /// Gets a value indicating whether [purchase link is set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [purchase link is set]; otherwise, <c>false</c>.
        /// </value>
        public bool PurchaseLinkIsSet
        {
            get { return PurchaseLink != null; }
        }

        /// <summary>
        /// Gets a value indicating whether [company information is set].
        /// </summary>
        /// <value>
        /// <c>true</c> if [company information is set]; otherwise, <c>false</c>.
        /// </value>
        public bool CompanyInfoIsSet
        {
            get { return CompanyText != null; }
        }
        #endregion
    }
}