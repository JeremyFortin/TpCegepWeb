using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TpCegepWeb.Controllers;

namespace TpCegepWeb.Controllers
{
    [TestClass]
    public class UnitTest1
    {
         
        public void Index_Retourne_View_Avec_Cegeps()
        {
            // Arrange
            var controller = new CegepController();

            // Simuler des données pour le ViewBag (comme si elles venaient de CegepControleur)
            var cegeps = new List<CegepDTO>
            {
                new CegepDTO { Nom = "Cegep 1", Adresse = "123 Rue A", Ville = "Ville A", Province = "Province A", CodePostal = "A1A 1A1", Telephone = "123-456-7890", Courriel = "cegep1@exemple.com" },
                new CegepDTO { Nom = "Cegep 2", Adresse = "456 Rue B", Ville = "Ville B", Province = "Province B", CodePostal = "B1B 1B1", Telephone = "987-654-3210", Courriel = "cegep2@exemple.com" }
            };

            controller.ViewBag.Cegeps = cegeps;

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result); // Vérifie que la vue n'est pas null
            Assert.Equal("Index", result?.ViewName); // Vérifie que le nom de la vue retournée est "Index"

            var cegepsFromViewBag = result?.ViewData["Cegeps"] as List<CegepDTO>;
            Assert.NotNull(cegepsFromViewBag); // Vérifie que ViewBag contient bien une liste de cégeps
            Assert.Equal(2, cegepsFromViewBag?.Count); // Vérifie qu'il y a 2 cégeps
        } 
    }
}
