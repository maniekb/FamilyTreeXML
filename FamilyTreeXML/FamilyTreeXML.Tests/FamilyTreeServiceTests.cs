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
            var family = LoadFamilyFromFile("TestData_1.xml");

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
            var family = LoadFamilyFromFile("TestData_1.xml");

            FamilyTreeService.AddFamily(family);
            var xdocs = FamilyTreeService.Browse();

            Assert.True(xdocs.Count == 1);
        }

        [Fact]
        public void recieved_family_should_be_the_same_as_saved_get()
        {
            var family = LoadFamilyFromFile("TestData_1.xml");
            FamilyTreeService.DeleteAll();

            FamilyTreeService.AddFamily(family);
            var expected = FamilyTreeService.Get(0);


            Assert.Equal(HelperClass.CreateFamily(family, 0).ToString(), expected.ToString());
        }

        [Fact]
        public void recieved_family_should_be_the_same_as_saved_browse()
        {
            var family = LoadFamilyFromFile("TestData_1.xml");
            FamilyTreeService.DeleteAll();

            FamilyTreeService.AddFamily(family);
            var expected = FamilyTreeService.Browse();

            Assert.Equal(HelperClass.CreateFamily(family, 0).ToString(), expected[0].ToString());
        }

        [Theory]
        [InlineData(Role.Daughter)]
        [InlineData(Role.Son)]
        public void adding_child_to_family_should_add_child_to_family_in_db(Role role)
        {
            var family = LoadFamilyFromFile("TestData_3.xml");
            FamilyTreeService.DeleteAll();

            var child = new Person
            {
                Role = role,
                Firstname = "firstname",
                Lastname = "lastname"
            };

            FamilyTreeService.AddFamily(family);
            FamilyTreeService.AddChild(0, child);

            var xdoc = FamilyTreeService.Get(0);
            var smth = $"{role}";
            var assertion = xdoc.Elements($"{role}").Any();

            Assert.True(xdoc.Descendants().Elements($"{role.ToString()}").Any());
        }

        [Fact]
        public void delete_should_remove_family_with_given_id_from_db()
        {
            FamilyTreeService.DeleteAll();
            var family = LoadFamilyFromFile("TestData_1.xml");

            FamilyTreeService.AddFamily(family);

            FamilyTreeService.Delete(0);

            var xdoc = FamilyTreeService.Get(0);

            Assert.True(xdoc.Root == null);
        }


        [Fact]
        public void delete_all_should_remove_all_families_from_db()
        {
            FamilyTreeService.DeleteAll();
            var family = LoadFamilyFromFile("TestData_1.xml");

            FamilyTreeService.AddFamily(family);
            FamilyTreeService.AddFamily(family);

            FamilyTreeService.DeleteAll();

            var xdocs = FamilyTreeService.Browse();

            Assert.True(!xdocs.Any());
        }

        [Fact]
        public void getfamilydaboville_should_return_correct_value()
        {
            FamilyTreeService.DeleteAll();
            var family0 = LoadFamilyFromFile("TestData_1.xml");
            var family1 = LoadFamilyFromFile("TestData_2.xml");
            var family2 = LoadFamilyFromFile("TestData_3.xml");

            FamilyTreeService.AddFamily(family0);
            FamilyTreeService.AddChild(0, new Person { Role = Role.Son, Firstname = "Lebron", Lastname = "James" });
            FamilyTreeService.AddFamily(family1);
            FamilyTreeService.AddChild(1, new Person { Role = Role.Daughter, Firstname = "Savannah", Lastname = "Bryant" });
            FamilyTreeService.AddFamily(family2);
            FamilyTreeService.AddChild(2, new Person { Role = Role.Son, Firstname = "Dwyane", Lastname = "James" });

            var expected = "1. Grandpa James & Granny James\n   1.1. Lebron James\n      1.1.1. Dwyane James\n";

            var actual = FamilyTreeService.GetFamilyDAboville(0);

            Assert.Equal(expected, actual);
        }

        public Family LoadFamilyFromFile(string file)
        {
            var xdoc = XDocument.Load(file);

            var family = new Family
            {
                FatherFamilyId = Int32.Parse(xdoc.Element("Tree").Element("Family").Attribute("fatherFamilyId").Value),
                MotherFamilyId = Int32.Parse(xdoc.Element("Tree").Element("Family").Attribute("motherFamilyId").Value),
                Father = xdoc.Descendants("Father").FirstOrDefault(),
                Mother = xdoc.Descendants("Mother").FirstOrDefault()
            };

            return family;
        }
    }
}
