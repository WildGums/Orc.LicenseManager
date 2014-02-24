// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Product : ICreateDate, ICreator
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        [ScaffoldColumn(false)]
        public string PassPhrase { get; set; }
        [ScaffoldColumn(false)]
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public virtual ICollection<LicensePoco> Licenses { get; set; }
        #endregion

        #region ICreateDate Members
        public DateTime CreationDate { get; set; }
        #endregion

        [ScaffoldColumn(false)]

        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
    }
}