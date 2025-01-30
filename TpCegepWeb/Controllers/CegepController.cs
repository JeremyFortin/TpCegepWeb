using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Logics.Modeles;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TpCegepWeb.Controllers
{
    public class CegepController : Controller
    {

        private List<CegepDTO> Cegeps;

        public CegepController()
        {
            Cegeps = CegepControleur.Instance.ObtenirListeCegep();
        }

        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Cegeps = Cegeps;
            return View();
        }

       
    }
}
