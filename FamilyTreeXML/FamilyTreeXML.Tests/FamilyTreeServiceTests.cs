using FamilyTreeXML.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace FamilyTreeXML.Tests
{
    public class FamilyTreeServiceTests
    {
        private readonly IFamilyTreeService FamilyTreeService = new FamilyTreeService("Data Source=DESKTOP-M13QDOP;Initial Catalog=FamilyTreeX;Integrated Security=True");
        string file = "C:\\Users\\Maciek\\Desktop\\.NET\\FamilyTreeXML\\FamilyTreeXML\\FamilyTreeXML.Tests\\TestData_1.xml";

        [Fact]
        public void no_ids_should_be_return_from_empty_db()
        {
            FamilyTreeService.DeleteAll();
            var ids = FamilyTreeService.GetFamilyIds();

            Assert.True(!ids.Any());
        }

        [Fact]
        public void one_id_should_be_return_from_db_with_one_family()
        {
            FamilyTreeService.DeleteAll();
            //var family = LoadFamilyFromFile("TestData_1.xml");
            var family = LoadFamilyFromFile(file);

            FamilyTreeService.AddFamily(family);
            var ids = FamilyTreeService.GetFamilyIds();

            Assert.True(ids.Count == 1);
        }

        [Fact]
        public void no_xdoc_should_be_return_from_empty_db()
        {
            FamilyTreeService.DeleteAll();
            var xdocs = FamilyTreeService.Browse();

            Assert.True(!xdocs.Any());
        }

        [Fact]
        public void one_xdoc_should_be_return_from_db_with_one_family()
        {
            FamilyTreeService.DeleteAll();
            //var family = LoadFamilyFromFile("TestData_1.xml");
            var family = LoadFamilyFromFile(file);

            FamilyTreeService.AddFamily(family);
            var xdocs = FamilyTreeService.Browse();

            Assert.True(xdocs.Count == 1);
        }

        [Fact]
        public void recieved_family_should_be_the_same_as_saved_get()
        {
            //var family = LoadFamilyFromFile("TestData_1.xml");
            var family = LoadFamilyFromFile(file);

            FamilyTreeService.AddFamily(family);
            var expected = FamilyTreeService.Get(0);


            Assert.Equal(HelperClass.CreateFamily(family, 0).ToString(), expected.ToString());
        }

        [Fact]
        public void recieved_family_should_be_the_same_as_saved_browse()
        {
            //var family = LoadFamilyFromFile("TestData_1.xml");
            var family = LoadFamilyFromFile(file);

            FamilyTreeService.AddFamily(family);
            var expected = FamilyTreeService.Browse();


            Assert.Equal(HelperClass.CreateFamily(family, 0).ToString(), expected[0].ToString());
        }

        public Family LoadFamilyFromFile(string file)
        {
            var xdoc = XDocument.Load(file);

            var family = new Family
            {
                FatherFamilyId = 0,
                MotherFamilyId = 0,
                Father = xdoc.Descendants("Father").FirstOrDefault(),
                Mother = xdoc.Descendants("Mother").FirstOrDefault()
            };

            return family;
        }
    }
}
