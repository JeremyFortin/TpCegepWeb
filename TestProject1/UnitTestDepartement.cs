using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;
using TpCegepWeb.Controllers;

namespace TestProject1
{
    public class UnitTestDepartement
    {
        private DepartementController controller = new DepartementController();

        List<CegepDTO> cegeps = CegepControleur.Instance.ObtenirListeCegep();

        

        [Fact]
        public void TestCegepValide()
        {
            var selectedCegep = "Cegep1";
            var result = controller.Index(selectedCegep) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Cegep1", selectedCegep);
            Assert.NotNull(result.ViewData["Departements"]);
            Assert.IsType<List<DepartementDTO>>(result.ViewData["Departements"]);
        }

        [Fact]
        public void TestCegepNull()
        {
            string selectedCegep = null;

            var result = controller.Index(selectedCegep) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["Departements"]);
            Assert.IsType<List<DepartementDTO>>(result.ViewData["Departements"]);
        }

        [Fact]
        public void TestCegepInexistant()
        {
            var selectedCegep = "CegepInexistant";

            var result = controller.Index(selectedCegep) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("CegepInexistant", selectedCegep);
            Assert.NotNull(result.ViewData["Departements"]);
            Assert.Empty((List<DepartementDTO>)result.ViewData["Departements"]);
        }
    }


}