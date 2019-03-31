using DFM1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFM1.ViewModels
{
    public class HomeViewModel
    {
        public string Title { get; set; }

        public List<Furniture> Furnitures { get; set; }
    }
}
