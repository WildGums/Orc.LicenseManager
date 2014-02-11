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
        /// Gets or sets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public string Website { get; set; }
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public string Key { get; set; }

        /// <summary>
        /// Gets a value indicating whether [website is set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [website is set]; otherwise, <c>false</c>.
        /// </value>
        public bool WebsiteIsSet
        {
            get { return Website != null; }
        }
        #endregion
    }
}