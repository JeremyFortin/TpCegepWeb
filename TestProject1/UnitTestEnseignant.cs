using GestionCegepWeb.Models;
using GestionCegepWeb.Logics.Controleurs;
using Microsoft.AspNetCore.Mvc;

using TpCegepWeb.Controllers;

namespace TestProject1
{
    public class UnitTestEnseignantController
    {
        private readonly EnseignantController controller;
        private readonly List<CegepDTO> cegeps;
        private readonly List<DepartementDTO> departements;
        private readonly List<EnseignantDTO> enseignants;

        public UnitTestEnseignantController()
        {
            // Initialisation dans le constructeur
            controller = new EnseignantController();

            cegeps = CegepControleur.Instance.ObtenirListeCegep();
            departements = CegepControleur.Instance.ObtenirListeDepartement(cegeps[0].Nom);
            enseignants = CegepControleur.Instance.ObtenirListeEnseignant(cegeps[0].Nom, departements[0].Nom);
        }

        [Fact]
        public void TestIndexAvecCegepEtDepartementValides()
        {
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = departements[0].Nom;

            var result = controller.Index(selectedCegep, selectedDepartement) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ListeCegeps"]);
            Assert.IsType<List<CegepDTO>>(result.ViewData["ListeCegeps"]);
            var cegepList = (List<CegepDTO>)result.ViewData["ListeCegeps"];
            Assert.Contains(cegeps[0], cegepList);

            Assert.NotNull(result.ViewData["ListeDepartements"]);
            Assert.IsType<List<DepartementDTO>>(result.ViewData["ListeDepartements"]);
            var departementList = (List<DepartementDTO>)result.ViewData["ListeDepartements"];
            Assert.Contains(departements[0], departementList); 

            Assert.NotNull(result.ViewData["ListeEnseignants"]);
            Assert.IsType<List<EnseignantDTO>>(result.ViewData["ListeEnseignants"]);
            var enseignantList = (List<EnseignantDTO>)result.ViewData["ListeEnseignants"];
            Assert.Contains(enseignants[0], enseignantList); 
        }

        [Fact]
        public void TestIndexAvecCegepEtDepartementNull()
        {
            var result = controller.Index(null, null) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ListeCegeps"]);
            Assert.IsType<List<CegepDTO>>(result.ViewData["ListeCegeps"]);
            var cegepList = (List<CegepDTO>)result.ViewData["ListeCegeps"];
            Assert.NotEmpty(cegepList); 

            Assert.NotNull(result.ViewData["ListeDepartements"]);
            Assert.IsType<List<DepartementDTO>>(result.ViewData["ListeDepartements"]);
            var departementList = (List<DepartementDTO>)result.ViewData["ListeDepartements"];
            Assert.NotEmpty(departementList); 
        }

        [Fact]
        public void TestAjouterEnseignant()
        {
            var enseignantDTO = new EnseignantDTO { Nom = "EnseignantTest", Prenom = "Test", NoEmploye = 12345 };
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = departements[0].Nom;

            var result = controller.AjouterEnseignant(selectedCegep, selectedDepartement, enseignantDTO) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Enseignant", result.ControllerName);
        }

        [Fact]
        public void TestFormulaireModifierEnseignant()
        {
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = departements[0].Nom;
            int noEnseignant = enseignants[0].NoEmploye;

            var result = controller.FormulaireModifierEnseignant(selectedCegep, selectedDepartement, noEnseignant) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<EnseignantDTO>(result.Model);
            var enseignant = (EnseignantDTO)result.Model;
            Assert.Equal(noEnseignant, enseignant.NoEmploye);
        }

        [Fact]
        public void TestModifierEnseignant()
        {
            var enseignantDTO = new EnseignantDTO { Nom = "EnseignantModifie", Prenom = "Modifie", NoEmploye = enseignants[0].NoEmploye };
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = departements[0].Nom;

            var result = controller.ModifierEnseignant(selectedCegep, selectedDepartement, enseignantDTO) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Enseignant", result.ControllerName);
        }

        [Fact]
        public void TestAjouterEnseignantAvecErreur()
        {
            var enseignantDTO = new EnseignantDTO { Nom = "EnseignantErreur", Prenom = "Erreur", NoEmploye = 99999 };
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = departements[0].Nom;

            CegepControleur.Instance.AjouterEnseignant(selectedCegep, selectedDepartement, enseignantDTO);

            var result = controller.AjouterEnseignant(selectedCegep, selectedDepartement, enseignantDTO) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Enseignant", result.ControllerName);
        }
    }
}
