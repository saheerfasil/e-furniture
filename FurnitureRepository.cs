using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFM1.Models
{
    public class FurnitureRepository : IFurnitureRepository
    {
        private readonly AppDbContext _appDbContext;

        public FurnitureRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Furniture> GetAllFurniture()
        {
            return _appDbContext.Furnitures;
        }

        public Furniture GetFurnitureById(int furnitureId)
        {
            return _appDbContext.Furnitures.FirstOrDefault(P => P.Id == furnitureId);
        }
    }
}
