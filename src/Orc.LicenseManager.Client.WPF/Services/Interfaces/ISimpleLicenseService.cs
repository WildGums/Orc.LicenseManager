// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISimpleLicenseService.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Services
{
    /// <summary>
    /// A very simple implementation of the license service.
    /// </summary>
    public interface ISimpleLicenseService
    {
        /// <summary>
        /// Validates the license in a very simple manner. This method is wrapper around the <see cref="ILicenseService" />.
        /// </summary>
        /// <param name="applicationId">The application identifier, can be any value but should be unique.</param>
        /// <param name="aboutTitle">The about title.</param>
        /// <param name="aboutImage">The about image.</param>
        /// <param name="aboutText">The about text.</param>
        /// <param name="aboutSiteUrl">The about site.</param>
        /// <param name="title">The title.</param>
        /// <param name="purchaseLinkUrl">The purchase link.</param>
        /// <returns><c>true</c> if the license is valid, <c>false</c> otherwise.</returns>
        /// <remarks>Note that this method might show a dialog so must be run on the UI thread.</remarks>
        bool Validate(string applicationId, string aboutTitle, string aboutImage, string aboutText, string aboutSiteUrl = null, string title = null, string purchaseLinkUrl = null);
    }
}