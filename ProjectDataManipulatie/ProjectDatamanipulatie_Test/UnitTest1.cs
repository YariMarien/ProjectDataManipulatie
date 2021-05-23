using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectDataManipulatie_DAL;
using System;

namespace ProjectDatamanipulatie_Test
{
    [TestClass]
    public class ValidPersonTest
    {
        [TestMethod]
        public void TestPersonValidation_Invalid()
        {
            Persoon p = new Persoon();
            bool geldig = p.IsGeldig();
            Assert.AreEqual(false, geldig);
        }
        //Niet de beste oplossing om de errors te testen.
        //Betere manier is dat ik met een enum zou werken.
        [TestMethod]
        public void TestPersonValidation_EmptyEmail()
        {
            Persoon p = new Persoon();


            bool isGeldig = p.IsGeldig();

            Assert.AreEqual(false, isGeldig);
            Assert.IsTrue(p.Error.Contains("Email is verplicht in te vullen."));

        }

        [TestMethod]
        public void TestPersonValidation_InvalidEmail()
        {
            Persoon p = new Persoon() {
            email = "abc"
            };


            bool isGeldig = p.IsGeldig();


            Assert.AreEqual(false, isGeldig);
            Assert.IsTrue(p.Error.Contains("Ongeldig email adres."));

        }
        [TestMethod]
        public void TestPersonValidation_InvalidBirthDate()
        {
            Persoon p = new Persoon()
            {
                geboorteDatum = DateTime.Now
            };


            bool isGeldig = p.IsGeldig();


            Assert.AreEqual(false, isGeldig);
            Assert.IsTrue(p.Error.Contains("Je moet minstens 6 jaar oud zijn."));

        }
        [TestMethod]
        public void TestPersonValidation_ValidPerson()
        {
            Persoon p = new Persoon()
            {
                geboorteDatum = new DateTime(2010, 01, 01),
                email = "geldig.mailadres@telenet.be"
            };


            bool isGeldig = p.IsGeldig();


            Assert.AreEqual(true, isGeldig);
            Assert.IsTrue(string.IsNullOrEmpty(p.Error));

        }
    }
}
