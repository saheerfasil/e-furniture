using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFM.Models
{
    public interface IFurnitureRepository
    {
            IEnumerable<Furniture> GetAllFurniture();

            Furniture GetFurnitureById(int furnitureId);
    }
}
