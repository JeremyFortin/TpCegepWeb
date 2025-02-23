using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;
using TpCegepWeb.Controllers;

namespace TestProject1
{
    /// <summary>
    /// Test unitaire pour les cours
    /// </summary>
    public class UnitTestCours
    {
        private readonly CoursController controller;
        private readonly List<CegepDTO> cegeps;
        private readonly List<DepartementDTO> departements;

        /// <summary>
        /// constructeur du test unitaire
        /// </summary>
        public UnitTestCours()
        {
            controller = new CoursController();
            cegeps = CegepControleur.Instance.ObtenirListeCegep();
            departements = CegepControleur.Instance.ObtenirListeDepartement(cegeps[0].Nom);
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void TestCegepValide()
        {
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = departements[0].Nom;

            var result = controller.Index(selectedCegep, selectedDepartement) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(selectedCegep, cegeps[0].Nom);
            Assert.NotNull(result.ViewData["ListeCours"]);
            Assert.IsType<List<CoursDTO>>(result.ViewData["ListeCours"]);
            var coursList = (List<CoursDTO>)result.ViewData["ListeCours"];
            Assert.NotEmpty(coursList);
        }

        [Fact]
        public void TestCegepNull()
        {
            var result = controller.Index(null, null) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ListeCours"]);
            Assert.IsType<List<CoursDTO>>(result.ViewData["ListeCours"]);
            var coursList = (List<CoursDTO>)result.ViewData["ListeCours"];
            Assert.Empty(coursList);
        }

        [Fact]
        public void TestCegepInexistant()
        {
            var result = controller.Index("cegep inexistant", "departement inexistant") as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("cegep inexistant", "departement inexistant");
            Assert.NotNull(result.ViewData["ListeCours"]);
            var coursList = (List<CoursDTO>)result.ViewData["ListeCours"];
            Assert.Empty(coursList);
        }

        [Fact]
        public void TestDepartementVide()
        {
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = "";

            var result = controller.Index(selectedCegep, selectedDepartement) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["ListeCours"]);
            Assert.IsType<List<CoursDTO>>(result.ViewData["ListeCours"]);
            var coursList = (List<CoursDTO>)result.ViewData["ListeCours"];
            Assert.Empty(coursList);
        }

        [Fact]
        public void TestAjouterCours()
        {
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = departements[0].Nom;
            var coursDTO = new CoursDTO
            {
                Nom = "Nouveau Cours",
                No = "CS101",
            };

            var result = controller.AjouterCours(selectedCegep, selectedDepartement, coursDTO) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public void TestModifierCours()
        {
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = departements[0].Nom;
            var coursDTO = new CoursDTO
            {
                Nom = "Cours Modifié",
                No = "CS102",
            };

            var result = controller.ModifierCours(selectedCegep, selectedDepartement, coursDTO) as RedirectToActionResult;

            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }
    }
}
