namespace CareManagement.Models.CRM
{
    using CareManagement.Models.OM;
    using MessagePack;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class Asset
    {
        public Guid AssetId { get; set; }
        public string Description { get; set; }
        public Enum.AssetType AssetType  { get; set; }
        [ForeignKey("Renter")]
        public Guid RenterId { get; set; }
        public string PlateNumber { get; set; }
        public string SuiteNo { get; set; }
    
        //public virtual AssetType AssetType { get; set; }
        public virtual Renter? Renter { get; set; }
    }
}
