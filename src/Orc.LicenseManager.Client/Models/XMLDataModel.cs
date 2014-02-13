using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.LicenseManager.Models
{
    /// <summary>
    /// XMLDataModel is used for extracting the data from a XML file
    /// </summary>
    public class XMLDataModel
    {
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
