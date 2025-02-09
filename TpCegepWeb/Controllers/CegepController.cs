using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace TpCegepWeb.Controllers
{
    public class CegepController : Controller
    {

        private List<CegepDTO> Cegeps;

        public CegepController()
        {
            Cegeps = CegepControleur.Instance.ObtenirListeCegep();
            if (Cegeps == null)
            {
                Cegeps.Add(new CegepDTO());
            }
        }

        [Route("")]
        [Route("/cegeps")]
        [Route("/cegeps/index")]
        public IActionResult Index()
        {
            ViewBag.Cegeps = Cegeps;
            return View();
        }

       
       
    }
}
