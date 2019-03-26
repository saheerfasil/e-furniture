using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DFM.Models;
using DFM.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DFM.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFurnitureRepository _furnitureRepository;

        public HomeController(IFurnitureRepository furnitureRepository)
        {
            _furnitureRepository = furnitureRepository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var furnishers = _furnitureRepository.GetAllFurniture().OrderBy(p => p.Name);

            var homeViewModel = new HomeViewModel()
            {
                Title = "SUNSHINE WOODWORKZ",
                Furnitures = furnishers.ToList()
            };
            return View(homeViewModel);
        }
    }
}
