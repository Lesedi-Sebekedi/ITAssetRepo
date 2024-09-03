using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITAssetRepo.Models
{
    public class Asset_list
    {
        [Key]
        [DisplayName("Asset Number")]
        public required string Asset_Number { get; set; }
        public string Description { get; set; }
        public string Catergory { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Acquire Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Acq_Date { get; set; }

        public string Location { get; set; }
        public string Label { get; set; }
        public string Custodian { get; set; }
        public string Condition { get; set; }
        public string PO_Number { get; set; }
        public string Model { get; set; }
        [DisplayName("Serial Number")]
        public string Serial_Number { get; set; }
        [DisplayName("Asset Cost")]
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Asset Cost must be a positive number.")]
        public decimal Asset_Cost { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public IFormFile TechnicalInspectionFile { get; set; }
        public string TechnicalInspectionFilePath { get; set; }

        [NotMapped]
        public IFormFile BitlockerFile { get; set; }
        public string BitlockerFilePath { get; set; }
    }
}
