namespace CareManagement.Models.CRM
{
    using System;
    using System.Collections.Generic;
    
    public partial class Applicant
    {
        public int ApplicantId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string ContactingNumber { get; set; }
        public string SharingInfo { get; set; }
        public double Income { get; set; }
        public string Employer { get; set; }
        public int AskingAssetType1 { get; set; }
        public Nullable<int> AskingAssetType2 { get; set; }
        public Nullable<int> AskingAssetType3 { get; set; }
        public Nullable<int> AskingAssetType4 { get; set; }
    
        public virtual AssetType AssetType { get; set; }
        public virtual AssetType AssetType1 { get; set; }
        public virtual AssetType AssetType2 { get; set; }
        public virtual AssetType AssetType3 { get; set; }
    }
}