using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TpCegepWeb.Controllers
{
    public class CegepController : Controller
    {

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
