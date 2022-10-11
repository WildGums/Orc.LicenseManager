namespace Orc.LicenseManager
{
    using Catel.Data;

    /// <summary>
    /// The license info model.
    /// </summary>
    public class LicenseInfo : ModelBase
    {
        public LicenseInfo(string title, string purchaseUrl, string infoUrl, string text, string imageUri, string? key = null)
        {
            Title = title;
            PurchaseUrl = purchaseUrl;
            InfoUrl = infoUrl;
            Text = text;
            ImageUri = imageUri;
            Key = key;
        }

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
        public string? Key { get; set; }
    }
}
