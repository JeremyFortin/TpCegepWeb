using Microsoft.AspNetCore.Mvc;

namespace TpCegepWeb.Controllers
{
    public class CoursController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
