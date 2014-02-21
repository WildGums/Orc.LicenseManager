// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LicensePoco.cs" company="Orchestra development team">
//   Copyright (c) 2008 - 2014 Orchestra development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.LicenseManager.Server
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class LicensePoco : IDates
    {
        #region Properties
        [Key]
        public int Id { get; set; }

        public string Value { get; set; }
        public string Comment { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
        #endregion

        #region IDates Members

        public DateTime ModificationDate { get; set; }

        public DateTime CreationDate { get; set; }
        #endregion
    }
}