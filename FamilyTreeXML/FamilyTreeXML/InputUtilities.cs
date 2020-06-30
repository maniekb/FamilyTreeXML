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
        public static Person GetPersonData(Role role)
        {
            char choice;
            string firstname;
            string lastname;
            DateTime birthDate;

            Console.WriteLine("Firstame: ");
            firstname = Console.ReadLine();

            Console.WriteLine("Lastname: ");
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
                Role = role
            };

        }

        public static XElement GetParentDataFromXDoc(XDocument xdoc, Role role)
        {
            var parent = new XElement("Init");
            int parentId = -1;

            if(role == Role.Father)
            {
                var sons = xdoc.Root.Element("Family").Elements("Son").ToList();
                Console.WriteLine($"Choose father(type number)");

                for (var i = 0; i < sons.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {sons[i].Value}");
                }
                parentId = int.Parse(Console.ReadLine());
                parent = sons[parentId - 1];
            }

            else if(role == Role.Mother)
            {
                var daughters = xdoc.Root.Element("Family").Elements("Daughter").ToList();
                Console.WriteLine("Choose mother(type number)");
                for (var i = 0; i < daughters.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {daughters[i].Value}");
                }
                parentId = int.Parse(Console.ReadLine());
                parent = daughters[parentId - 1];
            }

            return parent;
        }

        internal static XElement PersonToXElement(Person person)
        {
            XElement xelem = new XElement(person.Role.ToString(),
                new XElement("Firstname", person.Firstname),
                new XElement("Lastname", person.Lastname),
                new XElement("BirthDate", person.BirthDate.ToString(@"yyyy-MM-dd"))
            );

            return xelem;
        }
    }
}
