using Microsoft.AspNetCore.Mvc;

namespace BackOfficeFrontendService.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Empty home page to be redirected to
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }
    }
}