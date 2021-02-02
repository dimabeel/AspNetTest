using System.ComponentModel.DataAnnotations;

namespace AutoLotDAL.Models
{
    class InventoryMetaData
    {
        [Display(Name = "Pet Name")]
        public string PetName;

        [StringLength(50, ErrorMessage = "Please enter a value less than 50 characters long.")]
        public string Make;
    }
}
