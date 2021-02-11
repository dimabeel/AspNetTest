using System.Linq;
using System.Collections.Generic;
using AutoLotDALCore.EF;
using AutoLotDALCore.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.EF;


namespace AutoLotDALCore.Repos
{
    public class InventoryRepo : BaseRepo<Inventory>, IInventoryRepo
    {
        public InventoryRepo(AutoLotContext context) : base(context)
        {

        }

        public List<Inventory> Search(string searchString)
            => Context.Cars.Where(c => Functions.Like(c.PetName,
                $"%{searchString}%")).ToList();

        public List<Inventory> GetRelatedData()
            => Context.Cars.FromSqlRaw("SELECT * FROM Inventory")
            .Include(x => x.Orders)
            .ThenInclude(x => x.Customer)
            .ToList();

        public override List<Inventory> GetAll()
            => GetAll(x => x.PetName, true).ToList();

        public List<Inventory> GetPinkCars() => GetSome(x => x.Color == "Pink");
    }
}
