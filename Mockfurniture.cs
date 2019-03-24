using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFM.Models
{
    public class MockFurnitureRepository:IFurnitureRepository
    {
            private List<Furniture> _furniture;

            public MockFurnitureRepository()
            {
                if (_furniture == null)
                {
                    InitializeFurniture();
                }
            }

            private void InitializeFurniture()
            {
                _furniture = new List<Furniture>
                {
                    new Furniture {Id = 1, Name = "Table", Price = 12.95M, ShortDescription = "Our famous Table Model!", LongDescription = "Made of Malasian Teak with Indian ancient design. The hinges are made of brass. cake.", ImageUrl = "", IsFurnitureOfTheWeek = true, ImageThumbnailUrl = "/images/Thumbnails/Table.jpg"},
                    new Furniture {Id = 2, Name = "King Size Bed", Price = 18.95M, ShortDescription = "You'll like it!", LongDescription = "Made of Malasian Teak with Indian ancient design. The hinges are made of brass.Please fill more as you like............", ImageUrl = "", IsFurnitureOfTheWeek = false, ImageThumbnailUrl = "/images/Thumbnails/KingSizeBed.jpg"},
                    new Furniture {Id = 3, Name = "Book Shelf", Price = 18.95M, ShortDescription = "Plain Book Shelf..", LongDescription = "Made of Malasian Teak with Indian ancient design. The hinges are made of brass.Please fill more as you like............", ImageUrl = "", IsFurnitureOfTheWeek = false, ImageThumbnailUrl = "/images/Thumbnails/BookShelf.jpg"},
                    new Furniture {Id = 4, Name = "Chest of Drawers", Price = 15.95M, ShortDescription = "Ever Green classic!", LongDescription = "Made of Malasian Teak with Indian ancient design. The hinges are made of brass. Please fill more as you like............",ImageUrl = "",IsFurnitureOfTheWeek = false, ImageThumbnailUrl = "/images/Thumbnails/Chestofdrawers.jpg"}
                };
            }

            public IEnumerable<Furniture> GetAllFurniture()
            {
                return _furniture;
            }

            public Furniture GetFurnitureById(int furnitureId)
            {
                return _furniture.FirstOrDefault(p => p.Id == furnitureId);
            }
        }
}
