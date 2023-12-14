﻿namespace Orc.LicenseManager.Server;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product : ICreateDate, ICreator
{
    public int Id { get; set; }
    public string Name { get; set; }

    [ScaffoldColumn(false)]
    public string PassPhrase { get; set; }

    [ScaffoldColumn(false)]
    public string PrivateKey { get; set; }

    public string PublicKey { get; set; }
    public virtual ICollection<LicensePoco> Licenses { get; set; }

    #region ICreateDate Members
    [ScaffoldColumn(false)]
    public DateTime CreationDate { get; set; }
    #endregion

    #region ICreator Members
    [ScaffoldColumn(false)]
    public string CreatorId { get; set; }

    [ForeignKey("CreatorId")]
    public User Creator { get; set; }
    #endregion
}