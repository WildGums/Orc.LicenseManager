namespace Orc.LicenseManager
{
    /// <summary>
    /// XMLDataModel is used for extracting the data from a XML file
    /// </summary>
    public class XmlDataModel
    {
        public XmlDataModel()
        {
            
        }

        public XmlDataModel(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }
}
