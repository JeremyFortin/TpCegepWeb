using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace TpCegepWeb.Controllers
{
    public class CegepController : Controller
    {
        /// <summary>
        /// M�thode de service appel� lors de l'action Index.
        /// R�les de l'action : 
        ///   -Afficher la liste des C�geps.
        ///   -Afficher le formulaire pour l'ajout d'un C�gep.
        /// </summary>
        /// <returns>ActionResult suite aux traitements des donn�es.</returns>
        [Route("")]
        [Route("Cegep")]
        [Route("Cegep/Index")]
        [HttpGet]
        public IActionResult Index()
        {
            //Mettre le if et son contenu en commentaire avant de lancer les tests fonctionnels...
            //Si erreur provenant d'une autre action...
            if (TempData["MessageErreur"] != null)
                ViewBag.MessageErreur = TempData["MessageErreur"];

            try
            {
                //Pr�paration des donn�es pour la vue...
                ViewBag.ListeCegeps = CegepControleur.Instance.ObtenirListeCegep().ToArray();
            }
            catch (Exception e)
            {
                ViewBag.MessageErreur = e.Message;
            }

            //Retour de la vue...
            return View();
        }

        /// <summary>
        /// M�thode de service appel� lors de l'action AjouterCegep.
        /// R�les de l'action : 
        ///   -Ajouter un C�gep.
        /// </summary>
        /// <param name="cegepDTO">Le DTO du C�gep.</param>
        /// <returns>IActionResult</returns>
        [Route("/Cegep/AjouterCegep")]
        [HttpPost]
        public IActionResult AjouterCegep([FromForm] CegepDTO cegepDTO)
        {
            try
            {
                CegepControleur.Instance.AjouterCegep(cegepDTO);
            }
            catch (Exception e)
            {
                //Mettre cette ligne en commentaire avant de lancer les tests fonctionnels
                TempData["MessageErreur"] = e.Message;
            }
       
            //Lancement de l'action Index...
            return RedirectToAction("Index", "Cegep");
        }
    }
}
