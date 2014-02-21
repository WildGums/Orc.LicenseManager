// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server
{
    using System;

    public class Product : ICreateDate
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string PassPhrase { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        #endregion

        #region ICreateDate Members
        public DateTime CreationDate { get; set; }
        #endregion
    }
}