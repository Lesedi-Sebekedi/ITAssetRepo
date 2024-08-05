﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ITAssetRepo.Models
{
    public class Asset_list
    {
        [Key]
        [DisplayName("Asset Number")]
        public required string Asset_Number { get; set; }
        public string Description { get; set; }
        public string Catergory { get; set; }
        [DisplayName("Acquire Date")]
        public string Acq_Date { get; set; }
        public string Location { get; set; }
        public string Label { get; set; }
        public string Custodian { get; set; }
        public string Condition { get; set; }
        public string PO_Number { get; set; }
        public string Model { get; set; }
        [DisplayName("Serial Number")]
        public string Serial_Number { get; set; }
        [DisplayName("Asset Cost")]
        public string Asset_Cost { get; set; }
        public bool IsActive { get; set; }
    }
}
