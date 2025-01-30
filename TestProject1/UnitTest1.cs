using GestionCegepWeb.Logics.Controleurs;
using GestionCegepWeb.Models;
using Microsoft.AspNetCore.Mvc;
using TpCegepWeb.Controllers;

namespace TestProject1
{
    public class UnitTest1
    {
        private DepartementController controller;

        public UnitTest1()
        {
            var cegeps = new List<CegepDTO>
            {
                new CegepDTO { Nom = "Cegep1" },
                new CegepDTO { Nom = "Cegep2" }
            };

            controller = new DepartementController();
        }

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