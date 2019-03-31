using System;
using System.Collection.Generic;
using System.Linq;
using System.Threading.Tasks;
using DFM1.Models;
using Microsoft.AspNetCore.Mvc;
namespace DFM1.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackController(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }
        
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Feedback feedback)
        {
            _feedbackRepository.AddFeedback(feedback);
            return RedirectToAction("FeedbackComplete");
        }
        public IActionResult FeedbackComplete()
        {
            return View();
        }
    }
}
