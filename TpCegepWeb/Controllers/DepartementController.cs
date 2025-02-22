﻿using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace TpCegepWeb.Controllers
{
    public class DepartementController : Controller
    {


        [Route("/departements")]
        [Route("/departements/index")]
        [HttpGet]
        public IActionResult Index([FromQuery] string SelectedCegep)
        {
            try
            {
                //Si aucun Cégep n'est préalablement sélectionné...
                if (SelectedCegep == null)
                {
                    SelectedCegep = CegepControleur.Instance.ObtenirListeCegep()[0].Nom;
                }

                //Préparation des données pour la vue...
                ViewBag.ListeCegeps = CegepControleur.Instance.ObtenirListeCegep();
                ViewBag.Cegep = CegepControleur.Instance.ObtenirCegep(SelectedCegep);
                ViewBag.ListeDepartements = CegepControleur.Instance.ObtenirListeDepartement(SelectedCegep);
            }
            catch (Exception e)
            {
                //Si le Cégep est un bon Cégep, on utilise le premier département...
                if ((ViewBag.Cegep != null) && (e.Message == "Erreur lors de l'obtention d'un département par son nom et son cégep..."))
                {
                    try
                    {
                        if (CegepControleur.Instance.ObtenirListeDepartement(SelectedCegep).Count > 0)
                        {
                            ViewBag.ListeDepartements = CegepControleur.Instance.ObtenirListeDepartement(SelectedCegep).ToArray();
                        }
                        else
                        {
                            ViewBag.Departement = new DepartementDTO();
                            ViewBag.ListeDepartements = new List<DepartementDTO>().ToArray();
                        }
                    }
                    catch (Exception ex)
                    {
                        ViewBag.MessageErreur = ex.Message;
                    }
                }
                else
                {
                    SelectedCegep = CegepControleur.Instance.ObtenirListeCegep()[0].Nom;
                    //Préparation des données pour la vue...
                    ViewBag.ListeCegeps = CegepControleur.Instance.ObtenirListeCegep();
                    ViewBag.Cegep = CegepControleur.Instance.ObtenirCegep(SelectedCegep);
                    ViewBag.ListeDepartements = CegepControleur.Instance.ObtenirListeDepartement(SelectedCegep);
                   
                }
            }

            //Retour de la vue...
            return View();
        }

        [Route("/Departement/AjouterDepartement")]
        [HttpPost]
        public IActionResult AjouterDepartement([FromForm] DepartementDTO departementDTO, [FromForm] string SelectedCegep)
        {
            if (SelectedCegep == null)
            {
                SelectedCegep = CegepControleur.Instance.ObtenirListeCegep()[0].Nom;
            }
            try
            {
                CegepControleur.Instance.AjouterDepartement(SelectedCegep, departementDTO);
            }
            catch (Exception e)
            {
                //Mettre cette ligne en commentaire avant de lancer les tests fonctionnels
                TempData["MessageErreur"] = e.Message;
            }

            //Lancement de l'action Index...
            return RedirectToAction("Index", "Departement");
        }

        /// <summary>
        /// Action FormulaireModifierCours.
        /// Permet d'afficher le formulaire pour la modification d'un Cours.
        /// </summary>
        /// <param name="nomCegep">Nom du Cégep.</param>
        /// /// <param name="nomDepartement">Nom du département.</param>
        /// /// <param name="nomCours">Nom du Cours.</param>
        /// <returns>IActionResult</returns>
        [Route("/Departement/FormulaireModifierDepartement")]
        [HttpGet]
        public IActionResult FormulaireModifierDepartement([FromQuery] string nomCegep, [FromQuery] string nomDepartement)
        {
            try
            {
                DepartementDTO departement = CegepControleur.Instance.ObtenirDepartement(nomCegep, nomDepartement);
                return View(departement);
            }
            catch
            {
                return RedirectToAction("Index", "Departement");
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
        [Route("/Departement/ModifierDepartement")]
        [HttpPost]
        public IActionResult ModifierDepartement([FromForm] string nomCegep, [FromForm] DepartementDTO departementDTO)
        {
            try
            {
                CegepControleur.Instance.ModifierDepartement(nomCegep, departementDTO);
                return RedirectToAction("Index", "Departement");
            }
            catch (Exception e)
            {
                TempData["MessageErreur"] = e.Message;
                return RedirectToAction("FormulaireModifierDepartement", "Departement", new { nomDepartement = departementDTO.Nom });
            }
        }



    }
}
