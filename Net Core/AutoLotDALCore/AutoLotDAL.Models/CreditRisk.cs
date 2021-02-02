using System.ComponentModel.DataAnnotations;
using AutoLotDAL.Models.Base;

namespace AutoLotDAL.Models
{
    public partial class CreditRisk : EntityBase
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }
    }
}
