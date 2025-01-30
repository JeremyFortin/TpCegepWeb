using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace TpCegepWeb.Controllers
{
    public class DepartementController : Controller
    {
        private List<CegepDTO> Cegeps;

        public DepartementController()
        {
            Cegeps = CegepControleur.Instance.ObtenirListeCegep();
            if (Cegeps == null)
            {
                Cegeps.Add(new CegepDTO());
            }
            //assure que la liste des département n'est pas null
            ViewBag.Departements = new List<DepartementDTO>();
        }


        [Route("departements")]
        [HttpGet]
        public IActionResult Index([FromForm] string SelectedCegep)
        {
            ViewBag.Cegeps = Cegeps;
            if(SelectedCegep == null) {
                ViewBag.Departements = CegepControleur.Instance.ObtenirListeDepartement(Cegeps[0].Nom);
            }
            else
            {
                ViewBag.Departements = CegepControleur.Instance.ObtenirListeDepartement(SelectedCegep);
            }
           
            return View();
        }

       /* [Route("departements")]
        [HttpPost]
        public IActionResult Index([FromQuery] string SelectedCegep)
        {
            ViewBag.Cegeps = Cegeps;
            ViewBag.Departements = CegepControleur.Instance.ObtenirListeDepartement(SelectedCegep);
            return View();
        }*/


    }
}
