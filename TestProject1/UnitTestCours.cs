using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;
using TpCegepWeb.Controllers;

namespace TestProject1
{
    public class UnitTestCours
    {
        private CoursController controller = new CoursController();


        List<CegepDTO> cegeps = CegepControleur.Instance.ObtenirListeCegep();


        [Fact]
        public void TestCegepValide()
        {
            string selectedCegep = cegeps[0].Nom;
            string selectedDepartement = CegepControleur.Instance.ObtenirListeDepartement(selectedCegep)[0].Nom;
            var result = controller.Index(selectedCegep, selectedDepartement) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("Cegep1", selectedCegep);
            Assert.NotNull(result.ViewData["Courss"]);
            Assert.IsType<List<CoursDTO>>(result.ViewData["Courss"]);
        }

        [Fact]
        public void TestCegepNull()
        {
            string selectedCegep = null;

            var result = controller.Index(selectedCegep, null) as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["Courss"]);
            Assert.IsType<List<CoursDTO>>(result.ViewData["Courss"]);
        }

        [Fact]
        public void TestCegepInexistant()
        {
            var result = controller.Index("cegep inexistant", "departement inexistant") as ViewResult;

            Assert.NotNull(result);
            Assert.Equal("cegep inexistant", "departement inexistant");
            Assert.NotNull(result.ViewData["Courss"]);
            Assert.Empty((List<CoursDTO>)result.ViewData["Courss"]);
        }
    }


}