using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace AutoLotDAL.Models
{
	[ModelMetadataType(typeof(InventoryMetaData))]
	public partial class Inventory
	{
		public override string ToString()
		{
			// Since the PetName column could be empty, supply
			// the default name of **No Name**.
			return $"{PetName ?? "**No Name**"} is a {Color} {Make} with ID {Id}.";
		}

	    [NotMapped]
	    public string MakeColor => $"{Make} + ({Color})";
    }
}