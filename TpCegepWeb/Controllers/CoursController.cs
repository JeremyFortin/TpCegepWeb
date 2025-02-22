﻿using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace TpCegepWeb.Controllers
{
    public class CoursController : Controller
    {
        [Route("Cours")]
        [Route("Cours/Index")]
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
                ViewBag.ListeCours = CegepControleur.Instance.ObtenirListeCours(nomCegep, nomDepartement).ToArray();
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
                    //Préparation des données pour la vue...
                    ViewBag.ListeCegeps = CegepControleur.Instance.ObtenirListeCegep();
                    ViewBag.Cegep = CegepControleur.Instance.ObtenirCegep(nomCegep);
                    ViewBag.ListeDepartements = CegepControleur.Instance.ObtenirListeDepartement(nomCegep);
                    ViewBag.Departement = CegepControleur.Instance.ObtenirDepartement(nomCegep, nomDepartement);
                    ViewBag.ListeCours = CegepControleur.Instance.ObtenirListeCours(nomCegep, nomDepartement).ToArray();
                }
            }

            //Retour de la vue...
            return View();
        }

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
                //Mettre cette ligne en commentaire avant de lancer les tests fonctionnels
                TempData["MessageErreur"] = e.Message;
            }

            //Lancement de l'action Index...
            return RedirectToAction("Index", "Cours");
        }

        /// <summary>
        /// Action FormulaireModifierCours.
        /// Permet d'afficher le formulaire pour la modification d'un Cours.
        /// </summary>
        /// <param name="nomCegep">Nom du Cégep.</param>
        /// /// <param name="nomDepartement">Nom du département.</param>
        /// /// <param name="nomCours">Nom du Cours.</param>
        /// <returns>IActionResult</returns>
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
        /// Action ModifierCegep.
        /// Permet de modifier un Cours.
        /// </summary>
        /// <param name="nomCegep">Nom du cégep</param>
        /// /// <param name="nomDepartement">Nom du cégep</param>
        /// /// <param name="coursDTO">Cours a modifier</param>
        /// <returns>ActionResult</returns>
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
            //Lancement de l'action Index...
            return RedirectToAction("Index", "Cours");
        }
    }
}
