// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Customer.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Customer : ICreator, IDates
    {
        #region Properties
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Company { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string City { get; set; }
        public string Postal { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        public virtual ICollection<LicensePoco> Licenses { get; set; }
        #endregion

        #region ICreator Members
        [ScaffoldColumn(false)]
        public string CreatorId { get; set; }
        [ScaffoldColumn(false)]
        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        #endregion

        #region IDates Members
        [ScaffoldColumn(false)]
        public DateTime ModificationDate { get; set; }

        [ScaffoldColumn(false)]
        public DateTime CreationDate { get; set; }
        #endregion
    }
}