using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;

//namespace des controleurs
namespace TpCegepWeb.Controllers
{
    /// <summary>
    /// classe représentant le controleur de vue des cours
    /// </summary>
    public class CoursController : Controller
    {
        /// <summary>
        /// méthode de service appelé lors de l'action index
        /// rôles de l'action 
        ///   afficher la liste des cours
        ///   afficher le formulaire pour l'ajout d un cours
        /// </summary>
        /// <param name="nomCegep">nom du cégep</param>
        /// <param name="nomDepartement">nom du département</param>
        /// <returns>actionresult suite aux traitements des données</returns>
        [Route("Cours")]
        [Route("Cours/Index")]
        [HttpGet]
        public IActionResult Index([FromQuery] string nomCegep, [FromQuery] string nomDepartement)
        {
            try
            {
                //si aucun cégep département n'est préalablement sélectionné
                if ((nomCegep is null) || (nomDepartement is null))
                {
                    nomCegep = CegepControleur.Instance.ObtenirListeCegep()[0].Nom;
                    nomDepartement = CegepControleur.Instance.ObtenirListeDepartement(nomCegep)[0].Nom;
                }

                //préparation des données pour la vue
                ViewBag.ListeCegeps = CegepControleur.Instance.ObtenirListeCegep();
                ViewBag.Cegep = CegepControleur.Instance.ObtenirCegep(nomCegep);
                ViewBag.ListeDepartements = CegepControleur.Instance.ObtenirListeDepartement(nomCegep);
                ViewBag.Departement = CegepControleur.Instance.ObtenirDepartement(nomCegep, nomDepartement);
                ViewBag.ListeCours = CegepControleur.Instance.ObtenirListeCours(nomCegep, nomDepartement).ToArray();
            }
            catch (Exception e)
            {
                //si le cégep est un bon cégep on utilise le premier département
                if ((ViewBag.Cegep != null) && (e.Message == "Erreur lors de l'obtention d'un département par son nom et son cégep..."))
                {
                    try
                    {
                        if (CegepControleur.Instance.ObtenirListeDepartement(nomCegep).Count > 0)
                        {
                            nomDepartement = CegepControleur.Instance.ObtenirListeDepartement(nomCegep)[0].Nom;
                            ViewBag.Departement = CegepControleur.Instance.ObtenirDepartement(nomCegep, nomDepartement);
                            ViewBag.ListeCours = CegepControleur.Instance.ObtenirListeCours(nomCegep, nomDepartement).ToArray();
                        }
                        else
                        {
                            nomDepartement = "";
                            ViewBag.Departement = new DepartementDTO();
                            ViewBag.ListeCours = new List<CoursDTO>().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.MessageErreur = ex.Message;
                    }
                }
                else
                {
                    nomCegep = CegepControleur.Instance.ObtenirListeCegep()[0].Nom;
                    nomDepartement = CegepControleur.Instance.ObtenirListeDepartement(nomCegep)[0].Nom;
                    // préparation des données pour la vue
                    ViewBag.ListeCegeps = CegepControleur.Instance.ObtenirListeCegep();
                    ViewBag.Cegep = CegepControleur.Instance.ObtenirCegep(nomCegep);
                    ViewBag.ListeDepartements = CegepControleur.Instance.ObtenirListeDepartement(nomCegep);
                    ViewBag.Departement = CegepControleur.Instance.ObtenirDepartement(nomCegep, nomDepartement);
                    ViewBag.ListeCours = CegepControleur.Instance.ObtenirListeCours(nomCegep, nomDepartement).ToArray();
                }
            }

            //retour de la vue
            return View();
        }

        /// <summary>
        /// méthode de service appelé lors de l'action ajoutercours
        /// rôles de l'action 
        ///   ajouter un cours
        /// </summary>
        /// <param name="nomCegep">nom du cégep</param>
        /// <param name="nomDepartement">nom du département</param>
        /// <param name="coursDTO">dto du cours</param>
        /// <returns>iactionresult</returns>
        [Route("/Cours/AjouterCours")]
        [HttpPost]
        public IActionResult AjouterCours([FromForm] string nomCegep, [FromForm] string nomDepartement, [FromForm] CoursDTO coursDTO)
        {
            try
            {
                CegepControleur.Instance.AjouterCours(nomCegep, nomDepartement, coursDTO);
            }
            catch (Exception e)
            {
                // mettre cette ligne en commentaire avant de lancer les tests fonctionnels
                TempData["MessageErreur"] = e.Message;
            }

            // lancement de l'action index
            return RedirectToAction("Index", "Cours");
        }

        /// <summary>
        /// action formulairemodifiercours
        /// permet d afficher le formulaire pour la modification d'un cours
        /// </summary>
        /// <param name="nomCegep">nom du cégep</param>
        /// <param name="nomDepartement">nom du département</param>
        /// <param name="nomCours">nom du cours</param>
        /// <returns>iactionresult</returns>
        [Route("/Cours/FormulaireModifierCours")]
        [HttpGet]
        public IActionResult FormulaireModifierCours([FromQuery] string nomCegep, [FromQuery] string nomDepartement, [FromQuery] string nomCours)
        {
            try
            {
                ViewBag.MessageErreur = TempData["MessageErreur"];
                CoursDTO cours = CegepControleur.Instance.ObtenirCours(nomCegep, nomDepartement, nomCours);
                ViewBag.NomDepartement = nomDepartement;
                ViewBag.NomCegep = nomCegep;
                return View(cours);
            }
            catch
            {
                return RedirectToAction("Index", "Cours");
            }

        }

        /// <summary>
        /// action modifiercours
        /// permet de modifier un cours
        /// </summary>
        /// <param name="nomCegep">nom du cégep</param>
        /// <param name="nomDepartement">nom du département</param>
        /// <param name="coursDTO">cours a modifier</param>
        /// <returns>actionresult</returns>
        [Route("/Cours/ModifierCours")]
        [HttpPost]
        public IActionResult ModifierCours([FromForm] string nomCegep, [FromForm] string nomDepartement, [FromForm] CoursDTO coursDTO)
        {
            try
            {
                CegepControleur.Instance.ModifierCours(nomCegep, nomDepartement, coursDTO);
            }
            catch (Exception e)
            {
                TempData["MessageErreur"] = e.Message;
                return RedirectToAction("FormulaireModifierCours", "Cours", new { nomCours = coursDTO.Nom });
            }
            // lancement de l'action index
            return RedirectToAction("Index", "Cours");
        }
    }
}