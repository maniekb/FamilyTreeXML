using FamilyTreeXML.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

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
                gender = Console.ReadLine()[0];
                if (Char.ToLower(gender) == 'm' || Char.ToLower(gender) == 'f')
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
    }
}
