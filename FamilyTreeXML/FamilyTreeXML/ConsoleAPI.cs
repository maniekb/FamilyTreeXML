﻿using FamilyTreeXML.Infrastructure;
using System;
using System.Linq;
using System.Xml.Linq;

namespace FamilyTreeXML.App
{
    class ConsoleAPI
    {
        private readonly IFamilyTreeService FamilyTreeService;
        private XDocument xdoc;

        public ConsoleAPI(IFamilyTreeService familyTreeService)
        {
            FamilyTreeService = familyTreeService;
        }

        public void Run()
        {
            char choice;
            int id;

            while(true)
            {
                Console.WriteLine(@"
    FamilyTreeXML
----- OPTIONS -----
1 - Get family tree (d'Aboville system)
2 - Browse all families
3 - Get family with id
4 - Create new family
5 - Add child to family
6 - Get number of kids in every family
7 - Get person birth date
8 - Delete family with id
9 - Delete all families
Q - QUIT
                ");

                choice = Console.ReadLine()[0];
                Console.Clear();

                switch (choice)
                {
                    case '1':
                        var familyIds = FamilyTreeService.GetFamilyIds();
                        Console.WriteLine("Insert family id.");
                        var familyId = Convert.ToInt32(Console.ReadLine());
                        if (!familyIds.Contains(familyId))
                        {
                            Console.WriteLine("No family with given id.");
                            break;
                        }
                        var ftree = FamilyTreeService.GetFamilyDAboville(familyId);
                        Console.WriteLine(ftree);
                        break;
                    case '2':
                        var trees = FamilyTreeService.Browse();
                        if(trees.Any())
                        {
                            foreach (var tree in trees)
                            {
                                Console.WriteLine(tree.ToString());
                            }
                        }
                        else
                        {
                            Console.WriteLine("No trees in the database.");
                        }
                        break;
                    case '3':
                        Console.WriteLine("Insert id:");
                        id = Convert.ToInt32(Console.ReadLine());
                        xdoc = FamilyTreeService.Get(id);
                        if (xdoc.Root == null)
                        {
                            Console.WriteLine("No family with given id.");
                            break;
                        }
                        Console.WriteLine(xdoc.ToString());                     
                        break;
                    case '4':
                        Person father;
                        Person mother;
                        XDocument fatherFamily;
                        XDocument motherFamily;

                        Family newFamily = new Family();

                        Console.WriteLine("Is father family arleady in DB? (Y/N)");
                        choice = Console.ReadLine()[0];
                        if(choice == 'n')
                        {
                            father = InputUtilities.GetPersonData(Role.Father);
                            newFamily.FatherFamilyId = -1;
                            newFamily.Father = InputUtilities.PersonToXElement(father);
                        }
                        else if(choice == 'y')
                        {
                            var ids = FamilyTreeService.GetFamilyIds();
                            Console.WriteLine("Insert father family id.");
                            var fatherFamilyId = Convert.ToInt32(Console.ReadLine());
                            if (!ids.Contains(fatherFamilyId))
                            {
                                Console.WriteLine("No family with given id.");
                                break;
                            }
                            fatherFamily = FamilyTreeService.Get(fatherFamilyId);
                            newFamily.Father = InputUtilities.GetParentDataFromXDoc(fatherFamily, Role.Father);
                            newFamily.FatherFamilyId = fatherFamilyId;
                        }
                        else
                        {
                            Console.WriteLine("Wrong input.");
                            break;
                        }

                        Console.WriteLine("Is mother family arleady in DB? (Y/N)");
                        choice = Console.ReadLine()[0];
                        if (choice == 'n')
                        {
                            mother = InputUtilities.GetPersonData(Role.Mother);
                            newFamily.MotherFamilyId = -1;
                            newFamily.Mother = InputUtilities.PersonToXElement(mother);
                        }
                        else if (choice == 'y')
                        {
                            var ids = FamilyTreeService.GetFamilyIds();
                            Console.WriteLine("Insert mother family id.");
                            var motherFamilyId = Convert.ToInt32(Console.ReadLine());
                            if (!ids.Contains(motherFamilyId))
                            {
                                Console.WriteLine("No family with given id.");
                                break;
                            }
                            motherFamily = FamilyTreeService.Get(motherFamilyId);
                            newFamily.Mother = InputUtilities.GetParentDataFromXDoc(motherFamily, Role.Mother);
                            newFamily.MotherFamilyId = motherFamilyId;
                        }
                        else
                        {
                            Console.WriteLine("Wrong input.");
                            break;
                        }


                        FamilyTreeService.AddFamily(newFamily);

                        break;
                    case '5':
                        Console.WriteLine("Insert family id:");                
                        id = Convert.ToInt32(Console.ReadLine());
                        if(!FamilyTreeService.GetFamilyIds().Contains(id))
                        {
                            Console.WriteLine("No family with given id.");
                            break;
                        }
                        while (true)
                        {
                            Console.WriteLine("Son or daughter? (S/D): ");
                            choice = Char.ToLower(Console.ReadLine()[0]);
                            if (choice == 's' || choice == 'd')
                                break;
                            Console.WriteLine("Please, type S/D.");
                        }
                        var role = choice == 's' ? Role.Son : Role.Daughter;
                        var child = InputUtilities.GetPersonData(role);
                        FamilyTreeService.AddChild(id, child);
                        break;
                    case '6':
                        var data = FamilyTreeService.ChildInEveryFamilyCount();
                        foreach(var row in data)
                        {
                            Console.WriteLine($"Family with id {row.Item1}: {row.Item2}");
                        }
                        break;
                    case '7':
                        Console.WriteLine("Firstame: ");
                        var firstname = Console.ReadLine();
                        Console.WriteLine("Lastname: ");
                        var lastname = Console.ReadLine();

                        string birthdate = FamilyTreeService.GetPersonBirthDate(firstname, lastname);
                        if(!String.IsNullOrEmpty(birthdate))
                        {
                            Console.WriteLine($"{firstname} {lastname}'s birthdate is: {birthdate} ");
                        }
                        else
                        {
                            Console.WriteLine("No person found.");
                        }

                        break;
                    case '8':
                        Console.WriteLine("Insert id:");
                        int id3 = Convert.ToInt32(Console.ReadLine());
                        int rowsAffected = FamilyTreeService.Delete(id3);
                        if (rowsAffected == 0)
                        {
                            Console.WriteLine("Deletion couldn't be proceeded. There is no family with given id or " +
                                "id is referenced in another family XML document.");
                        }
                        break;
                    case '9':
                        FamilyTreeService.DeleteAll();
                        break;
                    case 'q':
                        Environment.Exit(0);
                        break;

                }
            }
        }
    }
}
