using Microsoft.AspNetCore.Mvc;
using SamplesSignalR.Models;

namespace SamplesSignalR.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = ViewModelBase.Default();
            return View(model);
        }
    }
}
