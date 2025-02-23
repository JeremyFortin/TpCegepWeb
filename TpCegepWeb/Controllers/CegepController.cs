using Microsoft.AspNetCore.Mvc;
using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;

/// <summary>
/// Namespace pour les controleurs de vue.
/// </summary>
namespace GestionCegepWeb.Controllers
{
    /// <summary>
    /// Classe repr�sentant le controleur de vue des C�geps.
    /// </summary>
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
            ViewBag.MessageErreur = TempData["MessageErreur"];

            try
            {
                //Pr�paration des donn�es pour la vue...
                ViewBag.ListeCegeps = CegepControleur.Instance.ObtenirListeCegep();
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

        /// <summary>
        /// Action FormulaireModifierCegep.
        /// Permet d'afficher le formulaire pour la modification d'un C�gep.
        /// </summary>
        /// <param name="nomCegep">Nom du C�gep.</param>
        /// <returns>IActionResult</returns>
        [Route("/Cegep/FormulaireModifierCegep")]
        [HttpGet]
        public IActionResult FormulaireModifierCegep([FromQuery] string nomCegep)
        {
            try
            {
                ViewBag.MessageErreur = TempData["MessageErreur"];
                CegepDTO cegep = CegepControleur.Instance.ObtenirCegep(nomCegep);
                return View(cegep);
            }
            catch
            {
                return RedirectToAction("Index", "Cegep");
            }

        }

        /// <summary>
        /// Action ModifierCegep.
        /// Permet de modifier un C�gep.
        /// </summary>
        /// <param name="cegepDTO">Le C�gep a modifier.</param>
        /// <returns>ActionResult</returns>
        [Route("/Cegep/ModifierCegep")]
        [HttpPost]
        public IActionResult ModifierCegep([FromForm] CegepDTO cegepDTO)
        {
            try
            {
                CegepControleur.Instance.ModifierCegep(cegepDTO);
            }
            catch (Exception e)
            {
                TempData["MessageErreur"] = e.Message;
                return RedirectToAction("FormulaireModifierCegep", "Cegep", new { nomCegep = cegepDTO.Nom });
            }
            //Lancement de l'action Index...
            return RedirectToAction("Index", "Cegep");
        }
    }
}
