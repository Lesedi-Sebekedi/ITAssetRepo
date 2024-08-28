using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ITAssetRepo.Models
{
    public class Asset
    {
        [Key]
        [DisplayName("Asset Number")]
        public required string Asset_Number { get; set; }
        public string Description { get; set; }
        public string Catergory { get; set; }
        [DisplayName("Acquire Date")]
<<<<<<< HEAD:ITAssetRepo/Models/Asset.cs
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Acq_Date { get; set; }

=======
        public string Acq_Date { get; set; }
>>>>>>> parent of 976e7e1 (partial upload):ITAssetRepo/Models/Asset_list.cs
        public string Location { get; set; }
        public string Label { get; set; }
        public string Custodian { get; set; }
        public string Condition { get; set; }
        public string PO_Number { get; set; }
        public string Model { get; set; }
        [DisplayName("Serial Number")]
        public string Serial_Number { get; set; }
        [DisplayName("Asset Cost")]
<<<<<<< HEAD:ITAssetRepo/Models/Asset.cs
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Asset Cost must be a positive number.")]
        public decimal Asset_Cost { get; set; }

=======
        public string Asset_Cost { get; set; }
>>>>>>> parent of 976e7e1 (partial upload):ITAssetRepo/Models/Asset_list.cs
        public bool IsActive { get; set; }
    }
}
