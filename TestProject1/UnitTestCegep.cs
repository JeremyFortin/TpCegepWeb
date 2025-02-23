using GestionCegepWeb.Controllers;
using GestionCegepWeb.Models;
using GestionCegepWeb.Logics.Controleurs;
using Microsoft.AspNetCore.Mvc;


namespace TestUnitaires
{
    public class UnitTestCegepController
    {
        private CegepController controller = new CegepController();
        private List<CegepDTO> cegeps = CegepControleur.Instance.ObtenirListeCegep();

        [Fact]
        public void TestIndexAvecCegepValide()
        {
            string selectedCegep = cegeps[0].Nom;

            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ListeCegeps"]);
            Assert.IsType<List<CegepDTO>>(result.ViewData["ListeCegeps"]);
            var cegepList = (List<CegepDTO>)result.ViewData["ListeCegeps"];
            Assert.Contains(cegeps[0], cegepList); 
        }

        [Fact]
        public void TestIndexAvecCegepNull()
        {
            var result = controller.Index() as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ListeCegeps"]);
            Assert.IsType<List<CegepDTO>>(result.ViewData["ListeCegeps"]);
            var cegepList = (List<CegepDTO>)result.ViewData["ListeCegeps"];
            Assert.NotEmpty(cegepList); 
        }

        [Fact]
        public void TestAjouterCegepDevraitRedirigerVersIndex()
        {
            var cegepDTO = new CegepDTO { Nom = "Cegep4" };

            var result = controller.AjouterCegep(cegepDTO) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Cegep", result.ControllerName);
        }

        [Fact]
        public void TestModifierCegepDevraitRedirigerVersIndex()
        {
            var cegepDTO = new CegepDTO { Nom = "Cegep2" };

            var result = controller.ModifierCegep(cegepDTO) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("Cegep", result.ControllerName);
        }

        [Fact]
        public void TestFormulaireModifierCegep()
        {
            string nomCegep = cegeps[0].Nom;

            var result = controller.FormulaireModifierCegep(nomCegep) as ViewResult;

            Assert.NotNull(result);
            Assert.IsType<CegepDTO>(result.Model);
            var cegep = (CegepDTO)result.Model;
            Assert.Equal(nomCegep, cegep.Nom);
        }

        [Fact]
        public void TestModifierCegepAvecErreurDevraitRedirigerVersFormulaireModifierCegep()
        {
            var cegepDTO = new CegepDTO { Nom = "CegepInexistant" };
            CegepControleur.Instance.ModifierCegep(cegepDTO); 

            var result = controller.ModifierCegep(cegepDTO) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("FormulaireModifierCegep", result.ActionName);
            Assert.Equal("Cegep", result.ControllerName);
        }
    }
}
