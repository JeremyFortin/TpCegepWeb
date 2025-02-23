using GestionCegepWeb.Models;
using GestionCegepWeb.Logics.Controleurs;
using Microsoft.AspNetCore.Mvc;
using TpCegepWeb.Controllers;

namespace TestUnitaires
{
    public class UnitTestDepartement
    {
        private DepartementController controller = new DepartementController();

        private readonly List<CegepDTO> Cegeps;
        private readonly List<DepartementDTO> Departements;

        public UnitTestDepartement()
        {
            Departements = CegepControleur.Instance.ObtenirListeDepartement(Cegeps[0].Nom);
            Cegeps = CegepControleur.Instance.ObtenirListeCegep();
        }

        [Fact]
        public void TestIndexAvecCegepValide()
        {
            string selectedCegep = Cegeps[0].Nom;
            var result = controller.Index(selectedCegep) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ListeCegeps"]);
            Assert.IsType<List<CegepDTO>>(result.ViewData["ListeCegeps"]);
            var cegepList = (List<CegepDTO>)result.ViewData["ListeCegeps"];
            Assert.Contains(Cegeps[0], cegepList);

            Assert.NotNull(result.ViewData["ListeDepartements"]);
            Assert.IsType<List<DepartementDTO>>(result.ViewData["ListeDepartements"]);
            var departementList = (List<DepartementDTO>)result.ViewData["ListeDepartements"];
            Assert.Contains(Departements[0], departementList);
        }

        [Fact]
        public void TestIndexAvecCegepNull()
        {
            var result = controller.Index(null) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ListeCegeps"]);
            Assert.IsType<List<CegepDTO>>(result.ViewData["ListeCegeps"]);
            var cegepList = (List<CegepDTO>)result.ViewData["ListeCegeps"];
            Assert.NotEmpty(cegepList);
        }

        [Fact]
        public void TestAjouterDepartement()
        {
            var departementDTO = new DepartementDTO { Nom = "DepartementTest" };
            string selectedCegep = Cegeps[0].Nom;

            var result = controller.AjouterDepartement(departementDTO, selectedCegep) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Departement", result.ControllerName);
        }

        [Fact]
        public void TestFormulaireModifierDepartement()
        {
            string selectedCegep = Cegeps[0].Nom;
            string selectedDepartement = Departements[0].Nom;

            var result = controller.FormulaireModifierDepartement(selectedCegep, selectedDepartement) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<DepartementDTO>(result.Model);
            var departement = (DepartementDTO)result.Model;
            Assert.Equal(selectedDepartement, departement.Nom);
        }

        [Fact]
        public void TestModifierDepartement()
        {
            var departementDTO = new DepartementDTO { Nom = "DepartementModifie" };
            string selectedCegep = Cegeps[0].Nom;

            var result = controller.ModifierCours(selectedCegep, departementDTO) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Departement", result.ControllerName);
        }

        [Fact]
        public void TestAjouterDepartementAvecErreur()
        {
            var departementDTO = new DepartementDTO { Nom = "DepartementInvalide" };
            string selectedCegep = Cegeps[0].Nom;

            CegepControleur.Instance.AjouterDepartement(selectedCegep, departementDTO);

            var result = controller.AjouterDepartement(departementDTO, selectedCegep) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Departement", result.ControllerName);
        }
    }
}
