using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;

//namespace des controllers
namespace TpCegepWeb.Controllers
{
    /// <summary>
    /// controller des enseignants
    /// </summary>
    public class EnseignantController : Controller
    {
        /// <summary>
        /// méthode de service qui affiche la liste des enseignant
        /// </summary>
        /// <param name="nomCegep">cégep sélectionner</param>
        /// <param name="nomDepartement">département sélectionné</param>
        /// <returns>un IActionResult</returns>
        [Route("Enseignant")]
        [Route("Enseignant/Index")]
        [HttpGet]
        public IActionResult Index([FromQuery] string nomCegep, [FromQuery] string nomDepartement)
        {
            try
            {
                //Si aucun Cégep/Département n'est préalablement sélectionné...
                if ((nomCegep is null) || (nomDepartement is null))
                {
                    nomCegep = CegepControleur.Instance.ObtenirListeCegep()[0].Nom;
                    nomDepartement = CegepControleur.Instance.ObtenirListeDepartement(nomCegep)[0].Nom;
                }

                //Préparation des données pour la vue...
                ViewBag.ListeCegeps = CegepControleur.Instance.ObtenirListeCegep();
                ViewBag.Cegep = CegepControleur.Instance.ObtenirCegep(nomCegep);
                ViewBag.ListeDepartements = CegepControleur.Instance.ObtenirListeDepartement(nomCegep);
                ViewBag.Departement = CegepControleur.Instance.ObtenirDepartement(nomCegep, nomDepartement);
                ViewBag.ListeEnseignants = CegepControleur.Instance.ObtenirListeEnseignant(nomCegep, nomDepartement);
            }
            catch (Exception e)
            {
                //Si le Cégep est un bon Cégep, on utilise le premier département...
                if ((ViewBag.Cegep != null) && (e.Message == "Erreur lors de l'obtention d'un département par son nom et son cégep..."))
                {
                    try
                    {
                        if (CegepControleur.Instance.ObtenirListeDepartement(nomCegep).Count > 0)
                        {
                            nomDepartement = CegepControleur.Instance.ObtenirListeDepartement(nomCegep)[0].Nom;
                            ViewBag.Departement = CegepControleur.Instance.ObtenirDepartement(nomCegep, nomDepartement);
                            ViewBag.ListeEnseignants = CegepControleur.Instance.ObtenirListeEnseignant(nomCegep, nomDepartement);
                        }
                        else
                        {
                            nomDepartement = "";
                            ViewBag.Departement = new DepartementDTO();
                            ViewBag.ListeEnseignants = new List<EnseignantDTO>();
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
                    //Préparation des données pour la vue...
                    ViewBag.ListeCegeps = CegepControleur.Instance.ObtenirListeCegep();
                    ViewBag.Cegep = CegepControleur.Instance.ObtenirCegep(nomCegep);
                    ViewBag.ListeDepartements = CegepControleur.Instance.ObtenirListeDepartement(nomCegep);
                    ViewBag.Departement = CegepControleur.Instance.ObtenirDepartement(nomCegep, nomDepartement);
                    ViewBag.ListeEnseignants = CegepControleur.Instance.ObtenirListeEnseignant(nomCegep, nomDepartement);
                }
            }

            //Retour de la vue...
            return View();
        }

        /// <summary>
        /// méthode qui permet d'ajouter un enseignant
        /// </summary>
        /// <param name="nomCegep">cegep sélectionné</param>
        /// <param name="nomDepartement">département sélectionné</param>
        /// <param name="enseignantDTO">enseignant a ajouté</param>
        /// <returns>un IActionResult</returns>
        [Route("/Enseignant/AjouterEnseignant")]
        [HttpPost]
        public IActionResult AjouterEnseignant([FromForm] string nomCegep, [FromForm] string nomDepartement, [FromForm] EnseignantDTO enseignantDTO)
        {
            try
            {
                CegepControleur.Instance.AjouterEnseignant(nomCegep,nomDepartement, enseignantDTO);
            }
            catch (Exception e)
            {
                //Mettre cette ligne en commentaire avant de lancer les tests fonctionnels
                TempData["MessageErreur"] = e.Message;
            }

            //Lancement de l'action Index...
            return RedirectToAction("Index", "Enseignant");
        }

        /// <summary>
        /// Action FormulaireModifierEnseignant.
        /// Permet d'afficher le formulaire pour la modification d'un Enseignant.
        /// </summary>
        /// <param name="nomCegep">Nom du Cégep.</param>
        /// /// <param name="nomDepartement">Nom du département.</param>
        /// /// <param name="nomEnseignant">Nom de l'enseignant.</param>
        /// <returns>IActionResult</returns>
        [Route("/Enseignant/FormulaireModifierEnseignant")]
        [HttpGet]
        public IActionResult FormulaireModifierEnseignant([FromQuery] string nomCegep, [FromQuery] string nomDepartement, [FromQuery] int noEnseignant)
        {
            try
            {
                ViewBag.MessageErreur = TempData["MessageErreur"];
                EnseignantDTO enseignant = CegepControleur.Instance.ObtenirEnseignant(nomCegep, nomDepartement, noEnseignant);
                ViewBag.NomDepartement = nomDepartement;
                ViewBag.NomCegep = nomCegep;
                return View(enseignant);
            }
            catch
            {
                return RedirectToAction("Index", "Enseignant");
            }

        }

        /// <summary>
        /// Action ModifierCegep.
        /// Permet de modifier un Enseignant.
        /// </summary>
        /// <param name="nomCegep">Nom du cégep</param>
        /// /// <param name="nomDepartement">Nom du cégep</param>
        /// /// <param name="enseignantDTO">Enseignant a modifier</param>
        /// <returns>ActionResult</returns>
        [Route("/Enseignant/ModifierEnseignant")]
        [HttpPost]
        public IActionResult ModifierEnseignant([FromForm] string nomCegep, [FromForm] string nomDepartement, [FromForm] EnseignantDTO enseignantDTO)
        {
            try
            {
                CegepControleur.Instance.ModifierEnseignant(nomCegep, nomDepartement, enseignantDTO);
            }
            catch (Exception e)
            {
                TempData["MessageErreur"] = e.Message;
                return RedirectToAction("FormulaireModifierEnseignant", "Enseignant", new { noEnseignant = enseignantDTO.NoEmploye });
            }
            //Lancement de l'action Index...
            return RedirectToAction("Index", "Enseignant");
        }
    }
}
