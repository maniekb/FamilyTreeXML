using FamilyTreeXML.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FamilyTreeXML.App
{
    public static class InputUtilities
    {
        public static Person GetChildData()
        {
            char gender;
            string firstname;
            string lastname;
            DateTime birthDate;

            while(true)
            {
                Console.WriteLine("Child gender (Type M/F): ");
                gender = Char.ToLower(Console.ReadLine()[0]);
                if (gender == 'm' || gender == 'f')
                    break;
                Console.WriteLine("Please, type M/F.");
            }

            Console.WriteLine("Child firstame: ");
            firstname = Console.ReadLine();

            Console.WriteLine("Child lastname: ");
            lastname = Console.ReadLine();

            Console.Write("Enter a year of birth: ");
            int year = int.Parse(Console.ReadLine());
            Console.Write("Enter a month of birth: ");
            int month = int.Parse(Console.ReadLine());
            Console.Write("Enter a day of birth: ");
            int day = int.Parse(Console.ReadLine());

            birthDate = new DateTime(year, month, day);

            return new Person
            {
                Firstname = firstname,
                Lastname = lastname,
                BirthDate = birthDate,
                Role = gender == 'm' ? Role.Son : Role.Daughter
            };

        }

        public static Family GetNewFamilyData(XDocument fatherFamily, XDocument motherFamily)
        {
            var family = new Family();

            // Get father
            var sons = fatherFamily.Root.Element("Family").Elements("Son").ToList();
            Console.WriteLine($"Choose father(type number)");

            for (var i = 0; i < sons.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {sons[i].Value}");
            }
            var fatherId = int.Parse(Console.ReadLine());

            // Get mother      
            var daughters = motherFamily.Root.Element("Family").Elements("Daughter").ToList();
            Console.WriteLine("Choose mother(type number)");
            for (var i = 0; i < daughters.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {daughters[i].Value}");
            }
            var motherId = int.Parse(Console.ReadLine());

            family.FatherFamilyId = int.Parse(fatherFamily.Root.Element("Family").Attribute("Id").Value);
            family.MotherFamilyId = int.Parse(motherFamily.Root.Element("Family").Attribute("Id").Value);
            family.Mother = daughters[motherId - 1];
            family.Father = sons[fatherId - 1];

            return family;
        }
    }
}
